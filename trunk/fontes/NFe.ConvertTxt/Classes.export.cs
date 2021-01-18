namespace NFe.ConvertTxt
{
    public class exportInd
    {
        public string nRE;
        public string chNFe;
        public double qExport;
    }

    public class detExport
    {
        public string nDraw;
        public exportInd exportInd;
        public detExport()
        {
            exportInd = new exportInd();
        }
    }
}
