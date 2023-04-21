using Classes.CellObjects;

namespace Interfaces;

public interface IAction {
    public int Invoke(Cell cell, int? argument);
}