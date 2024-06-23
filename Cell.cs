using System;
using System.Collections.Generic;

public class Cell
{
    public enum Status { EMPTY, SHIP, HIT, MISS }

    private Status status;

    public Cell()
    {
        status = Status.EMPTY;
    }

    public Status GetStatus()
    {
        return status;
    }

    public void SetStatus(Status newStatus)
    {
        status = newStatus;
    }
}
