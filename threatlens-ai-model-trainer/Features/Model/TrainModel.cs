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
                new TextLoader.Column("DstIp", DataKind.String, 1),
                new TextLoader.Column("Sport", DataKind.Int32, 2),
                new TextLoader.Column("Dsport", DataKind.Int32, 3),
                new TextLoader.Column("Proto", DataKind.String, 4),
                new TextLoader.Column("Sbytes", DataKind.Int32, 5),
                new TextLoader.Column("Dbytes", DataKind.Int32, 6),
                new TextLoader.Column("Sttl", DataKind.Int32, 7),
                new TextLoader.Column("Label", DataKind.Int32, 8)
            };

            var textLoader = mlContext.Data.CreateTextLoader(
                columns: columnDefinitions.ToArray(),
                separatorChar: '\t',
                hasHeader: false);

            var dataView = textLoader.Load(filePaths);

            // Simplified pipeline: only use the necessary features
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("SrcIpEncoded", "SrcIp")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("DstIpEncoded", "DstIp"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("ProtoEncoded", "Proto"))
                .Append(mlContext.Transforms.Conversion.ConvertType("SportFloat", "Sport", DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType("DsportFloat", "Dsport", DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType("SbytesFloat", "Sbytes", DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType("DbytesFloat", "Dbytes", DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType("SttlFloat", "Sttl", DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType("LabelBool", "Label", DataKind.Boolean))
                .Append(mlContext.Transforms.Concatenate("Features",
                    "SrcIpEncoded", "DstIpEncoded", "ProtoEncoded",
                    "SportFloat", "DsportFloat", "SbytesFloat", "DbytesFloat", "SttlFloat"))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: "LabelBool", featureColumnName: "Features"));

            var model = pipeline.Fit(dataView);

            string modelPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\dynamic_trained_model.zip";
            mlContext.Model.Save(model, dataView.Schema, modelPath);

            return 1;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
