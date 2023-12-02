using UnityEngine;
using System.Collections.Generic;
using StudyConceptsAPI;
using System.Linq;
using System.Collections;

public class GameController : MonoBehaviour
{

    //Stack setup and positioning
    [SerializeField]
    float _heightMultiplier = 1.005f, _widthMultiplier = 1.01f, _stackOffset = 15;

    [SerializeField]
    private JengaBlockObject _glassBlockPrefab, _woodBlockPrefab, _stoneBlockPrefab;
    [SerializeField]
    private JengaStackObject _stackPrefab;


    [SerializeField]
    private CameraController _cameraController;

    //Study data
    private StudyConcepts _concepts;
    private Dictionary<string, JengaStack> _jengaStacks;

    //UI
    [SerializeField]
    public UIOverlay UIOverlay;






    private void Awake()
    {
        _concepts = new StudyConcepts();
       
        _jengaStacks = new Dictionary<string, JengaStack>();

        AsyncInitialization();
    }

    // Use this for initialization
    void Start()
    {
    }

    private async void AsyncInitialization()
    {
        await _concepts.init();
        
        PopulateBlockConcepts();
        CreateStacks();
        
    }
    // Update is called once per frame
    void Update()
    {

  

    }

    void PopulateBlockConcepts()
    {
        foreach(var concept in _concepts.studyConcepts)
        {
            var grade = concept.Grade.Name;
            JengaStack stack = null;

            if (_jengaStacks.ContainsKey(grade))
                stack = _jengaStacks[grade];
            else
                stack = new JengaStack();

            var jengaBlock  = new JengaBlock(concept);
            stack.Name = grade;
            stack.AddBlock(jengaBlock);

            _jengaStacks[grade] = stack;
        }
    }

    void CreateStacks()
    {
        int stackIndex = 0;

        foreach(var stack in _jengaStacks.Values)
        {
            if (stack.JengaBlocks.Count < 10)
                continue;

            //Instantiate the "Stack" which acts as a parent for the blocks.
            int stackPositionZ = (stackIndex - 1) * -15;

            var stackObject = Instantiate(_stackPrefab, new Vector3(0, 0, stackPositionZ), Quaternion.identity);
            stackObject.JengaStack = stack;
            stack.GameObject = stackObject;
            _cameraController.TargetObjects.Add(stackObject.gameObject);

            stackIndex++;

            //Instantiate the blocks
            var initialRows = 3;
            var height = Mathf.CeilToInt(stack.JengaBlocks.Count / 3);

            if (height < 3)
                continue;


            var blocks = stack.JengaBlocks
                    .OrderBy(block => block.StudyConcept.Domain.Name)
                    .ThenBy(block => block.StudyConcept.Cluster.Name)
                    .ThenBy(block => block.StudyConcept.Standard.Id)
                    .ToList();

            var blockStack = new Stack<JengaBlock>(blocks);

            for (int rowPosition = 0; rowPosition < initialRows; rowPosition++)
            {
                for (int stackRow = 0; stackRow < height; stackRow++)
                {
                    int blockPosition = rowPosition - 1;

                    //Add a slight offset to avoid collision on instantiation
                    float positionX = stackRow % 2 == 0 ? blockPosition * _widthMultiplier : 0;
                    float positionY = stackRow * _heightMultiplier;
                    float positionZ = stackRow % 2 == 1 ? blockPosition * _widthMultiplier : 0;

                    Quaternion rotation = Quaternion.AngleAxis(90 * (stackRow % 2), Vector3.up);

                    JengaBlock block = blockStack.Pop();
                    if (block == null)
                        continue;
                    JengaBlockObject blockGameObject = null;
                    switch (block.Type)
                    {

                        case JengaBlock.BlockType.Stone:
                            blockGameObject = Instantiate(_stoneBlockPrefab, stackObject.transform.position + new Vector3(positionX, positionY, positionZ), rotation);    
                            break;
                        case JengaBlock.BlockType.Glass:
                            blockGameObject = Instantiate(_glassBlockPrefab, stackObject.transform.position + new Vector3(positionX, positionY, positionZ), rotation);
                            break;
                        case JengaBlock.BlockType.Wood:
                            blockGameObject = Instantiate(_woodBlockPrefab, stackObject.transform.position + new Vector3(positionX, positionY, positionZ), rotation);
                            break;
                    }

                    blockGameObject.JengaBlock = block;
                    block.GameObject = blockGameObject.gameObject;
                    blockGameObject.transform.parent = stackObject.transform;
                }
            }

        }
    }



    public bool ShowStackData(JengaStackObject jengaStack)
    {
        return false;
    }


    public bool ShowBlockData(JengaBlockObject jengaBlock)
    {

        if (jengaBlock == null)
        {
            UIOverlay.SetUITextFromBlock(null);
            return false;
        }

        JengaBlock block = jengaBlock.JengaBlock;

        if (block == null)
            return false;


        UIOverlay.SetUITextFromBlock(block);


        return false;
    }


    public void DestroyGlassBlocks()
    {

        JengaBlockObject[] jengaBlocks = FindObjectsOfType<JengaBlockObject>();

        foreach (var block in jengaBlocks)
        {
            // Turn off IsKinematic which turns their physics back on
            if (block.GetComponent<Rigidbody>() != null)
            {
                block.GetComponent<Rigidbody>().isKinematic = false;
            }

            // Destroy the block if its type is Glass
            if (block.JengaBlock.Type == JengaBlock.BlockType.Glass)
            {
                Destroy(block.gameObject);
            }
        }



    }


}
