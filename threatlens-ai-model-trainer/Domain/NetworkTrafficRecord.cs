using Microsoft.ML.Data;

namespace threatlens_ai_model_trainer.Domain
{
    public class NetworkTrafficRecord
    {
        [LoadColumn(0)] public string SrcIp { get; set; }
        [LoadColumn(1)] public int Sport { get; set; }
        [LoadColumn(2)] public string DstIp { get; set; }
        [LoadColumn(3)] public int Dsport { get; set; }
        [LoadColumn(4)] public string Proto { get; set; }
        [LoadColumn(5)] public string State { get; set; }
        [LoadColumn(6)] public float Dur { get; set; }
        [LoadColumn(7)] public int Sbytes { get; set; }
        [LoadColumn(8)] public int Dbytes { get; set; }
        [LoadColumn(9)] public int Sttl { get; set; }
        [LoadColumn(10)] public int Dttl { get; set; }
        [LoadColumn(11)] public int Sloss { get; set; }
        [LoadColumn(12)] public int Dloss { get; set; }
        [LoadColumn(13)] public string Service { get; set; }
        [LoadColumn(14)] public float Sload { get; set; }
        [LoadColumn(15)] public float Dload { get; set; }
        [LoadColumn(16)] public int Spkts { get; set; }
        [LoadColumn(17)] public int Dpkts { get; set; }
        [LoadColumn(18)] public int Swin { get; set; }
        [LoadColumn(19)] public int Dwin { get; set; }
        [LoadColumn(20)] public int Stcpb { get; set; }
        [LoadColumn(21)] public int Dtcpb { get; set; }
        [LoadColumn(22)] public int Smeansz { get; set; }
        [LoadColumn(23)] public int Dmeansz { get; set; }
        [LoadColumn(24)] public int TransDepth { get; set; }
        [LoadColumn(25)] public int ResBdyLen { get; set; }
        [LoadColumn(26)] public float Sjit { get; set; }
        [LoadColumn(27)] public float Djit { get; set; }
        [LoadColumn(28)] public long Stime { get; set; }
        [LoadColumn(29)] public long Ltime { get; set; }
        [LoadColumn(30)] public float Sintpkt { get; set; }
        [LoadColumn(31)] public float Dintpkt { get; set; }
        [LoadColumn(32)] public float Tcprtt { get; set; }
        [LoadColumn(33)] public float Synack { get; set; }
        [LoadColumn(34)] public float Ackdat { get; set; }
        [LoadColumn(35)] public int IsSmIpsPorts { get; set; }
        [LoadColumn(36)] public int CtStateTtl { get; set; }
        [LoadColumn(37)] public int CtFlwHttpMthd { get; set; }
        [LoadColumn(38)] public int IsFtpLogin { get; set; }
        [LoadColumn(39)] public int CtFtpCmd { get; set; }
        [LoadColumn(40)] public int CtSrvSrc { get; set; }
        [LoadColumn(41)] public int CtSrvDst { get; set; }
        [LoadColumn(42)] public int CtDstLtm { get; set; }
        [LoadColumn(43)] public int CtSrcLtm { get; set; }
        [LoadColumn(44)] public int CtSrcDportLtm { get; set; }
        [LoadColumn(45)] public int CtDstSportLtm { get; set; }
        [LoadColumn(46)] public int CtDstSrcLtm { get; set; }
        [LoadColumn(47)] public string AttackCat { get; set; }
        [LoadColumn(48)] public int Label { get; set; }
    }
}
