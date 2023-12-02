using StudyConceptsAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlay : MonoBehaviour
{

    public TextMeshProUGUI Grade;
    public TextMeshProUGUI Domain;
    public TextMeshProUGUI StandardDescription;
    public TextMeshProUGUI HelpText;
    public TextMeshProUGUI HelpButtonText;

    public void SetUITextFromBlock(JengaBlock block)
    {

        if (block == null)
        {
            Grade.text = "";

            Domain.text = "";

            StandardDescription.text = "";


        }
        else
        {
            Grade.text = block.StudyConcept.Grade.Name;

            Domain.text = block.StudyConcept.Domain.Name;

            StandardDescription.text = block.StudyConcept.Standard.Description;
        }
    }


    public void HideShowHelp()
    {
        HelpText.gameObject.SetActive(!HelpText.gameObject.activeSelf);
        if (HelpText.gameObject.activeSelf)
        {

            HelpButtonText.text = "Hide";
        }
        else
        {
            HelpButtonText.text = "Help";

        }

    }

}
