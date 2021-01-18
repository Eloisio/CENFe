using NFe.Components;

namespace NFe.Service
{
    public class TaskDFeRecepcaoCTe : TaskDFeRecepcao
    {
        public TaskDFeRecepcaoCTe(string arquivo)
            : base(arquivo)
        {
            Servico = Servicos.CTeDistribuicaoDFe;
        }

        public override void Execute()
        {
            base.Execute();
        }
    }
}