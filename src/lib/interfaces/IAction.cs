using Classes.CellObjects;

namespace Other;

public interface IAction {
    public int Invoke(Cell cell, int? argument);
}