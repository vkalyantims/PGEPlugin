using System;

public interface IViewController
{
    /// <summary>Fired when this view is “done” (e.g. user clicks Next).</summary>
    event Action OnComplete;
}