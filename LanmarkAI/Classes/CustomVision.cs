namespace LanmarkAI.Classes;

public class Prediction
{
    public string TagId { get; set; }
    public string Tag { get; set; }
    public double Probability { get; set; }
}

public class CustomVision
{
    public string Id { get; set; }
    public string Project { get; set; }
    public string Iteration { get; set; }
    public DateTime Created { get; set; }
    public List<Prediction> Predictions { get; set; }
}