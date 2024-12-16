using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using Threatlens.Application.Common;

namespace Threatlens.Application.Features.Model;

public class TrainModelCommandController : ApiControllerBase
{
    [HttpPost("/api/train-model")]
    public async Task<ActionResult<int>> Create(TrainModelCommand command)
    {
        return await Mediator.Send(command);
    }
}

public record TrainModelCommand() : IRequest<int>;

internal sealed class TrainModelCommandHandler() : IRequestHandler<TrainModelCommand, int>
{
    public async Task<int> Handle(TrainModelCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var mlContext = new MLContext();

            string baseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Threatlens\\Data\\";
            string[] filePaths = new[]
            {
                Path.Combine(baseFilePath, "UNSW-NB15_1.csv"),
                Path.Combine(baseFilePath, "UNSW-NB15_2.csv"),
                Path.Combine(baseFilePath, "UNSW-NB15_3.csv"),
                Path.Combine(baseFilePath, "UNSW-NB15_4.csv")
            };

            var columnDefinitions = new List<TextLoader.Column>
            {
                new TextLoader.Column("SrcIp", DataKind.String, 0),
                new TextLoader.Column("Sport", DataKind.Int32, 1),
                new TextLoader.Column("DstIp", DataKind.String, 2),
                new TextLoader.Column("Dsport", DataKind.Int32, 3),
                new TextLoader.Column("Proto", DataKind.String, 4),
                new TextLoader.Column("State", DataKind.String, 5),
                new TextLoader.Column("Dur", DataKind.Single, 6),
                new TextLoader.Column("Sbytes", DataKind.Int32, 7),
                new TextLoader.Column("Dbytes", DataKind.Int32, 8),
                new TextLoader.Column("Sttl", DataKind.Int32, 9),
                new TextLoader.Column("Dttl", DataKind.Int32, 10),
                new TextLoader.Column("Sloss", DataKind.Int32, 11),
                new TextLoader.Column("Dloss", DataKind.Int32, 12),
                new TextLoader.Column("Service", DataKind.String, 13),
                new TextLoader.Column("Sload", DataKind.Single, 14),
                new TextLoader.Column("Dload", DataKind.Single, 15),
                new TextLoader.Column("Spkts", DataKind.Int32, 16),
                new TextLoader.Column("Dpkts", DataKind.Int32, 17),
                new TextLoader.Column("Swin", DataKind.Int32, 18),
                new TextLoader.Column("Dwin", DataKind.Int32, 19),
                new TextLoader.Column("Stcpb", DataKind.Int32, 20),
                new TextLoader.Column("Dtcpb", DataKind.Int32, 21),
                new TextLoader.Column("Smeansz", DataKind.Int32, 22),
                new TextLoader.Column("Dmeansz", DataKind.Int32, 23),
                new TextLoader.Column("TransDepth", DataKind.Int32, 24),
                new TextLoader.Column("ResBdyLen", DataKind.Int32, 25),
                new TextLoader.Column("Sjit", DataKind.Single, 26),
                new TextLoader.Column("Djit", DataKind.Single, 27),
                new TextLoader.Column("Stime", DataKind.Int64, 28),
                new TextLoader.Column("Ltime", DataKind.Int64, 29),
                new TextLoader.Column("Sintpkt", DataKind.Single, 30),
                new TextLoader.Column("Dintpkt", DataKind.Single, 31),
                new TextLoader.Column("Tcprtt", DataKind.Single, 32),
                new TextLoader.Column("Synack", DataKind.Single, 33),
                new TextLoader.Column("Ackdat", DataKind.Single, 34),
                new TextLoader.Column("IsSmIpsPorts", DataKind.Int32, 35),
                new TextLoader.Column("CtStateTtl", DataKind.Int32, 36),
                new TextLoader.Column("CtFlwHttpMthd", DataKind.Int32, 37),
                new TextLoader.Column("IsFtpLogin", DataKind.Int32, 38),
                new TextLoader.Column("CtFtpCmd", DataKind.Int32, 39),
                new TextLoader.Column("CtSrvSrc", DataKind.Int32, 40),
                new TextLoader.Column("CtSrvDst", DataKind.Int32, 41),
                new TextLoader.Column("CtDstLtm", DataKind.Int32, 42),
                new TextLoader.Column("CtSrcLtm", DataKind.Int32, 43),
                new TextLoader.Column("CtSrcDportLtm", DataKind.Int32, 44),
                new TextLoader.Column("CtDstSportLtm", DataKind.Int32, 45),
                new TextLoader.Column("CtDstSrcLtm", DataKind.Int32, 46),
                new TextLoader.Column("AttackCat", DataKind.String, 47),
                new TextLoader.Column("Label", DataKind.Int32, 48)
            };

            var textLoader = mlContext.Data.CreateTextLoader(
                columns: columnDefinitions.ToArray(),
                separatorChar: '\t',
                hasHeader: false);

            var dataView = textLoader.Load(filePaths);

            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("SrcIpEncoded", "SrcIp")
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("DstIpEncoded", "DstIp"))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("ProtoEncoded", "Proto"))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("StateEncoded", "State"))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("ServiceEncoded", "Service"))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("AttackCatEncoded", "AttackCat"))
            .Append(mlContext.Transforms.Conversion.ConvertType("SportFloat", "Sport", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("DsportFloat", "Dsport", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("SbytesFloat", "Sbytes", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("DbytesFloat", "Dbytes", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("SttlFloat", "Sttl", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("DttlFloat", "Dttl", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("SlossFloat", "Sloss", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("DlossFloat", "Dloss", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("SpktsFloat", "Spkts", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("DpktsFloat", "Dpkts", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("SwinFloat", "Swin", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("DwinFloat", "Dwin", DataKind.Single))
            .Append(mlContext.Transforms.Conversion.ConvertType("LabelBool", "Label", DataKind.Boolean))
            .Append(mlContext.Transforms.Concatenate("Features",
                "SrcIpEncoded", "DstIpEncoded", "ProtoEncoded", "StateEncoded", "ServiceEncoded", "AttackCatEncoded",
                "SportFloat", "DsportFloat", "Dur", "SbytesFloat", "DbytesFloat", "SttlFloat", "DttlFloat",
                "SlossFloat", "DlossFloat", "Sload", "Dload", "SpktsFloat", "DpktsFloat",
                "SwinFloat", "DwinFloat"))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: "LabelBool", featureColumnName: "Features"));

            var model = pipeline.Fit(dataView);

            string modelPath = "dynamic_trained_model.zip";
            mlContext.Model.Save(model, dataView.Schema, modelPath);

            return 1;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
