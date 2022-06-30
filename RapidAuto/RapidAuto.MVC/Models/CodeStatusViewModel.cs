namespace RapidAuto.MVC.Models
{
    public class CodeStatusViewModel
    {
        public string MessageErreur { get; set; }
        public int Code { get; set; }
        public string IdRequete { get; set; }
        public bool AfficherId => !string.IsNullOrEmpty(IdRequete);
    }
}
