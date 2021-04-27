using NFe.Service;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NFe.Threadings
{
    /// Método que será executado ao processar o timer
    /// <param name="item"></param>
    internal delegate void ProcessarHandler(ThreadItem item);
    internal class BufferItem : IDisposable
    {
        #region propriedades
        /// Quando uma empresa usa um certificado do tipo A3, é executado por este Buffer
        public Queue<ThreadItem> Buffer { get; private set; }
        /// Este timer é executado sempre que o buffer possuir itens que deverão ser enviados
        private System.Timers.Timer timer = null;
        #endregion propriedades
        #region construtores
        /// Inicia uma nova instancia e define o método de processamento
        /// <param name="processar">método que será chamado para processar o item</param>
        public BufferItem(ProcessarHandler processar)
        {
            Buffer = new Queue<ThreadItem>();
            if (timer == null)
            {
                //-------------------------------------------------------------------------
                // Inicia o timer com 500 milissegundos. Desta forma a execução será rápida
                // Como faz um "Stop" logo ao iniciar o método "Elapsed", não tem problema
                // de chamar com este tempo.
                //-------------------------------------------------------------------------
                timer = new System.Timers.Timer(500);
                timer.Elapsed += (s, e) =>
                {
                    lock (Buffer)
                    {
                        timer.Stop();
                        try
                        {
                            // Recupera o threaditem e executa o mesmo
                            while (Buffer.Count > 0)
                            {
                                ThreadItem item = Buffer.Dequeue();
                                if (item == null) continue;
                                Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " iniciou o processamento pelo Buffer (Data criação: " + item.FileInfo.LastWriteTime + ")", false);
                                processar.Invoke(item);
                                Thread.Sleep(0);
                                Auxiliar.WriteLog("O arquivo " + item.FileInfo.FullName + " finalizou o processamento pelo Buffer (Data criação: " + item.FileInfo.LastWriteTime + ")", false);
                                if (Disposed) return;//cai fora ... foi descarregada
                            }
                        }
                        catch (Exception ex)
                        {
                            Auxiliar.WriteLog("ExceptionBuffer: " + ex.ToString(), false);
                        }
                        finally
                        {
                            // Sair ... mas continuar executando de tempos em tempos
                            timer.Start();
                        }
                    }
                };
            }
            timer.Start();
        }
        #endregion construtores
        #region IDisposable members
        /// Retorna true se este objeto foi descarregado
        public bool Disposed { get; private set; }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //faça algo
                GC.SuppressFinalize(this);
            }
            Disposed = true;
            timer.Stop();
            Buffer.Clear();
        }
        public virtual void Dispose()
        {
            Dispose(true);
        }
        #endregion IDisposable members
    }
    /// classe de item da thread
    public class ThreadItem : IDisposable
    {
        private static Dictionary<int, BufferItem> _buffer = null;
        /// Quando uma empresa usa um certificado do tipo A3, é executado por este Buffer
        /// É utilizado apenas os primeiros 8 dígitos do CNPJ como chave.
        private static IDictionary<int, BufferItem> Buffer
        {
            get
            {
                if (_buffer == null) _buffer = new Dictionary<int, BufferItem>();
                return _buffer;
            }
        }
        #region delegates
        public delegate void ThreadStartHandler(ThreadItem item);
        public delegate void ThreadEndedHandler(ThreadItem item);
        public delegate void ThreadReleasedHandler(ThreadItem item);
        #endregion delegates
        #region Eventos
        /// acontece quando a thread começou a leitura do arquivo
        public static event ThreadStartHandler OnStarted;
        /// acontece quando a thread finalizou a leitura do arquivo
        public static event ThreadEndedHandler OnEnded;
        /// acontece quando a thread removeu o arquivo do buffer
        public static event ThreadReleasedHandler OnReleased;
        #endregion Eventos
        public ThreadItem(System.IO.FileInfo fi, int empresa)
        {
            FileInfo = fi;
            Empresa = empresa;
            if (_buffer == null)
            {
                //criar um buffer para cada empresa que o certificado é A3
                foreach (Empresa emp in Empresas.Configuracoes)
                {
                    if (emp.UsaCertificado && emp.X509Certificado.IsA3())
                    {
                        // Usar o subject como chave, pois pode se configurar o mesmo certificado
                        // para empresas diferentes.
                        int key = emp.X509Certificado.Subject.GetHashCode();
                        AddToBuffer(key);
                    }
                }
            }
        }
        /// Este método adiciona o certificado a lista de buffer de certificados
        /// <param name="key">numero de chave do certificado</param>
        private void AddToBuffer(int key)
        {
            if (!Buffer.ContainsKey(key)) Buffer[key] = new BufferItem(Processar);
        }
        public System.IO.FileInfo FileInfo { get; private set; }
        public int Empresa { get; private set; }
        public void Run()
        {
            if (Empresas.Configuracoes.Count != 0)
            {
                Empresa empresa = Empresas.Configuracoes[Empresa];
                 // Se o certificado for A3, então vai para o Buffer controlado, pois deverá ser executado um de cada vez
                if (empresa.UsaCertificado && empresa.X509Certificado.IsA3())
                {
                    // Usar o subject como chave, pois pode se configurar o mesmo certificado
                    // para empresas diferentes.
                    int key = empresa.X509Certificado.Subject.GetHashCode();
                    AddToBuffer(key);
                    BufferItem bItem = Buffer[key];
                    bItem.Buffer.Enqueue(this);
                    return;
                }
            }
            Processar(this);
        }
        private void Processar(ThreadItem item)
        {
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
            {
                if (item.FileInfo.DirectoryName.ToLower().EndsWith("geral\\temp"))
                    item.Empresa = -1;
                Thread.CurrentThread.Name = item.Empresa.ToString();
            }
            try
            {
                //avisa que vai iniciar
                if (OnStarted != null) OnStarted(item);
                //avisa que vai finalizar
                if (OnEnded != null) OnEnded(item);
            }
            catch (Exception ex)
            {
                Auxiliar.WriteLog("Ocorreu um erro na execução da thread que está sendo executada.\r\nThreadControl.cs (1)\r\n" + ex.Message, true);
            }
            finally
            {
                try
                {
                    //remove o item
                    //avisa que removeu o item
                    if (OnReleased != null) OnReleased(item);
                }
                catch (Exception ex)
                {
                    Auxiliar.WriteLog("Ocorreu um erro ao tentar remover o item da Thread que está sendo executada.\r\nThreadControl.cs (2)\r\n" + ex.Message, true);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            Debug.WriteLine(String.Format("ThreadItem Dipose(disposing: {0});", disposing));
        }
    }
    /// Classe responsável por executar as thread´s base de verificação dos serviços a serem executados
    public static class ThreadService
    {
        public static List<Thread> Threads = new List<Thread>();
        public static void Stop()
        {
            for (int i = 0; i < Threads.Count; i++)
            {
                Thread t = Threads[i];
                t.Abort();
            }
            Threads.Clear();
            for (int i = 0; i < MonitoraPasta.fsw.Count; i++)
            {
                try
                {
                    FileSystemWatcher fsw = MonitoraPasta.fsw[i];
                    fsw.StopWatch = true;
                    fsw.Dispose();
                    fsw = null;
                }
                catch (Exception ex)
                {
                    Auxiliar.WriteLog("Ocorreu um erro ao tentar parar o FSW: " + MonitoraPasta.fsw[i].Directory + ".\r\nThreadService.cs\r\n" + ex.Message, false);
                }
            }
        }
        public static void Start()
        {
            TFunctions.CriarArquivosParaServico();

            Empresas.CarregaConfiguracao();

            #region Ticket #110

            Empresas.CreateLockFile(true);

            #endregion Ticket #110

            //Executar o monitoramento de pastas das empresas cadastradas
            MonitoraPasta e = new MonitoraPasta();

            Threads.Clear();

            //Executa a thread que faz a limpeza dos arquivos temporários
            Thread t = new Thread(new Processar().LimpezaTemporario);
            t.IsBackground = true;
            t.Start();
            Threads.Add(t);

            //Executa a thread que faz a verificação das notas em processamento
            Thread t2 = new Thread(new Processar().EmProcessamento);
            t2.IsBackground = true;
            t2.Start();
            Threads.Add(t2);

            //Executar a thread que faz a consulta do recibo das notas fiscais enviadas
            Processar srv = new Processar();
            Thread t3 = new Thread(srv.GerarXMLPedRec);
            t3.IsBackground = true;
            t3.Start(new TaskNFeGerarXMLPedRec());
            Threads.Add(t3);

            /*
            //Executa a thread que roda a manifestação do destinatário
            Thread t4 = new Thread(new Processar().ConsultaDFe);
            t4.IsBackground = true;
            t4.Start();
            Threads.Add(t4);
            */
        }
    }
}