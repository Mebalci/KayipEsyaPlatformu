namespace KayipEsyaPlatformu.Models
{
    public class Mahalle
    {
        public int Id { get; set; }              
        public int? SehirId { get; set; }        
        public int? IlceId { get; set; }         
        public int SemtId { get; set; }          
        public string Ad { get; set; }           
        public string PostaKodu { get; set; }    

        public Ilce? Ilce { get; set; }
        public Sehir? Sehir { get; set; }
    }


}
