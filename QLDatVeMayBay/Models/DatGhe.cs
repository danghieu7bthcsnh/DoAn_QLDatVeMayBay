namespace QLDatVeMayBay.Models
{
    public class DatGhe
    {
            public int IDChuyenBay { get; set; }
            public int TongSoGhe { get; set; }
            public List<int> GheDaDat { get; set; } = new();
        
    }

}

