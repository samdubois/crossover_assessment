
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JengaStackObject : MonoBehaviour
{

    public JengaStack JengaStack;
    [SerializeField]
    private TextMeshPro _label;

    private void Start()
    {
        _label.text = JengaStack.Name;
    }


}
public class JengaStack
{

    //public Stack<JengaBlock> GlassBlocks { get; } = new();
    //public Stack<JengaBlock> WoodBlocks { get; } = new();
    //public Stack<JengaBlock> StoneBlocks { get; } = new();

    public List<JengaBlock> JengaBlocks { get; } = new();
    public JengaStackObject GameObject { get; set; }
    public string Name;



    public void AddBlock(JengaBlock block)
    {
        //switch (block.Type)
        //{
        //    case JengaBlock.BlockType.Glass:
        //        GlassBlocks.Push(block);
        //        break;
        //    case JengaBlock.BlockType.Wood:
        //        WoodBlocks.Push(block);
        //        break;
        //    case JengaBlock.BlockType.Stone:
        //        StoneBlocks.Push(block);
        //        break;
        //}  
        JengaBlocks.Add(block);
    }



}
