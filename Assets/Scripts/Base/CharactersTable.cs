using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharactersTable
{
    List<CharacterRow> _rows;

    public List<CharacterRow> Rows { get => _rows; set => _rows = value; }

    public void Init()
    {
        _rows = new();
    }
    public CharacterRow AddRow(CharacterFacade character)
    {
        CharacterRow row = new CharacterRow();
        row.gameObject.Value = character.gameObject;
        row.character = character;
        row.isAlive.Value = true;

        Rows.Add(row);

        return row;
    }
}
public class RowBase
{
    public event Action onChange;

    public int num;

    public void ApplyChanges()
    {
        onChange?.Invoke();
    }
}
public class LiveCreatureRow : RowBase
{
    public LiveCreatureRow()
    {
        gameObject = new(this);
        spawner = new(this);

        isAlive = new BooleanTableCell(this);
        isAlive.Value = true;
    }

    public TableCell<GameObject> gameObject;
    public TableCell<Spawner> spawner;
    public BooleanTableCell isAlive;
}
public class CharacterRow : LiveCreatureRow
{
    public int numInGroup = -1;
    public CharacterFacade character;
    public Vector2Int publicTargetPos = -Vector2Int.one * int.MaxValue;
}
public class TableCell<T>
{
    public Action<RowBase, T> onChange;

    public TableCell(RowBase row)
    {
        this.row = row;
    }
    protected RowBase row;

    protected T value;

    public virtual T Value { get => value; set { this.value = value; onChange?.Invoke(row, value); row.ApplyChanges(); } }

}
public class BooleanTableCell : TableCell<bool>
{
    public BooleanTableCell(RowBase value) : base(value)
    {
    }
    public override bool Value { get => value; set { if (this.value == value) return; this.value = value; onChange?.Invoke(row, value); row.ApplyChanges(); } }
}


