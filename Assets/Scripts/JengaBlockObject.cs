
using StudyConceptsAPI;
using UnityEngine;

public class JengaBlockObject : MonoBehaviour
{
    public JengaBlock JengaBlock;

}

public class JengaBlock
{
    public StudyConcept StudyConcept { get; }

    public enum BlockType { Glass, Wood, Stone }
    public BlockType Type { get; set; }
    public GameObject GameObject { get; set; }

    public JengaBlock(StudyConcept concept)
    {
        StudyConcept = concept;
        Type = (BlockType)concept.Mastery;
    }


    override public string ToString()
    {
        return StudyConcept.ToString();

    }

}
