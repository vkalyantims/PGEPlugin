using System.Collections.Generic;

public interface IResults
{
    public List<Result> AllResults { get; set; }
    void AddResult(Result result);
}