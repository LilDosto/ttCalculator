using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject PNLMainMenu, PNLEquationRootFinder, PNLEquationSystemSolver, INPTEquation, INPTMinX, INPTMaxX,
        INPTRoot, INPTRoots, TGLNewtonRaphson, INPTNRInitialPoint, INPTEqVariable, INPTIterationCount, INPTEquationCount,
        INPTEquationPrefab, INPTVariablePrefab, INPTInitialPointPrefab, ESEquationParent, ESVariableParent, ESInitialPointParent,
        INPTESIterationCount;
    public string INPTRootText, INPTRootsText;
    public bool isNewtonRaphson;

    public void BTNEquationRootFinder()
    {
        PNLMainMenu.SetActive(false);
        PNLEquationRootFinder.SetActive(true);
        PNLEquationSystemSolver.SetActive(false);
    }
    public void BTNEquationSystemSolver()
    {
        PNLMainMenu.SetActive(false);
        PNLEquationRootFinder.SetActive(false);
        PNLEquationSystemSolver.SetActive(true);
    }

    public void BTNMainMenuERF()
    {
        PNLMainMenu.SetActive(true);
        PNLEquationRootFinder.SetActive(false);
        PNLEquationSystemSolver.SetActive(false);

        INPTNRInitialPoint.GetComponent<InputField>().text = "";
        INPTMaxX.GetComponent<InputField>().text = "";
        INPTMinX.GetComponent<InputField>().text = "";
        INPTEqVariable.GetComponent<InputField>().text = "";
        INPTIterationCount.GetComponent<InputField>().text = "";
        INPTEquation.GetComponent<InputField>().text = "";
    }
    public void BTNMainMenuESS()
    {
        PNLMainMenu.SetActive(true);
        PNLEquationRootFinder.SetActive(false);
        PNLEquationSystemSolver.SetActive(false);

        for (int i = 0; i < Equations.Count; i++)
        {
            Destroy(Equations[i]);
            Destroy(Variables[i]);
            Destroy(InitialPoints[i]);
        }

        Equations.Clear();
        Variables.Clear();
        InitialPoints.Clear();

        INPTEquationCount.GetComponent<InputField>().text = "0";
    }

    public void BTNFindEquationRoot()
    {
        string eq = INPTEquation.GetComponent<InputField>().text;
        string variable = INPTEqVariable.GetComponent<InputField>().text;
        int iterCount = int.Parse(INPTIterationCount.GetComponent<InputField>().text);

        decimal res = 0;

        if (isNewtonRaphson)
        {
            decimal initialP = decimal.Parse(INPTNRInitialPoint.GetComponent<InputField>().text);
            res = NumericalMethods.Method_NewtonRaphson(eq, variable, initialP, iterCount);
        }
        else
        {
            decimal minX = decimal.Parse(INPTMinX.GetComponent<InputField>().text);
            decimal maxX = decimal.Parse(INPTMaxX.GetComponent<InputField>().text);
            res = NumericalMethods.Method_BisectionMethod(eq, variable, minX, maxX, iterCount);
        }
        INPTRootText = res.ToString();
        INPTRoot.GetComponent<InputField>().text = res.ToString();
    }
    public void TGLNewtonRaphsonBisection()
    {
        if (TGLNewtonRaphson.GetComponent<Toggle>().isOn)
        {
            isNewtonRaphson = true;
            INPTNRInitialPoint.SetActive(true);
            INPTMaxX.SetActive(false);
            INPTMinX.SetActive(false);

            INPTNRInitialPoint.GetComponent<InputField>().text = "";
            INPTMaxX.GetComponent<InputField>().text = "";
            INPTMinX.GetComponent<InputField>().text = "";
        }
        else
        {
            isNewtonRaphson = false;
            INPTNRInitialPoint.SetActive(false);
            INPTMaxX.SetActive(true);
            INPTMinX.SetActive(true);

            INPTNRInitialPoint.GetComponent<InputField>().text = "";
            INPTMaxX.GetComponent<InputField>().text = "";
            INPTMinX.GetComponent<InputField>().text = "";
        }
    }

    public void INPTRootChanged()
    {
        INPTRoot.GetComponent<InputField>().text = INPTRootText;
    }
    public void INPTRootsChanged()
    {
        INPTRoots.GetComponent<InputField>().text = INPTRootsText;
    }

    List<GameObject> Equations = new List<GameObject>(), Variables = new List<GameObject>(), InitialPoints = new List<GameObject>();
    public void INPTEquationCountChanged()
    {
        int eqCount = INPTEquationCount.GetComponent<InputField>().text != "" ? int.Parse(INPTEquationCount.GetComponent<InputField>().text) : 0;

        for (int i = 0; i < Equations.Count; i++)
        {
            Destroy(Equations[i]);
            Destroy(Variables[i]);
            Destroy(InitialPoints[i]);
        }

        Equations.Clear();
        Variables.Clear();
        InitialPoints.Clear();

        for (int i = 0; i < eqCount; i++)
        {
            GameObject GO1 = Instantiate(INPTEquationPrefab);
            GO1.transform.SetParent(ESEquationParent.transform);
            GO1.transform.localScale = Vector3.one;
            Equations.Add(GO1);

            GameObject GO2 = Instantiate(INPTVariablePrefab);
            GO2.transform.SetParent(ESVariableParent.transform);
            GO2.transform.localScale = Vector3.one;
            Variables.Add(GO2);

            GameObject GO3 = Instantiate(INPTInitialPointPrefab);
            GO3.transform.SetParent(ESInitialPointParent.transform);
            GO3.transform.localScale = Vector3.one;
            InitialPoints.Add(GO3);
        }
    }

    Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
    public void BTNFindEquationSystemRoots()
    {
        List<string> Eqs = new List<string>();

        dict.Clear();
        for (int i = 0; i < Equations.Count; i++)
        {
            dict.Add(Variables[i].GetComponent<InputField>().text, decimal.Parse(InitialPoints[i].GetComponent<InputField>().text));
            Eqs.Add(Equations[i].GetComponent<InputField>().text);
        }

        int iterCount = int.Parse(INPTESIterationCount.GetComponent<InputField>().text);

        Dictionary<string, decimal> results = NumericalMethods.MethodMultiVar_SolveEquationSystem(Eqs, dict, iterCount);

        string sum = "";

        foreach (KeyValuePair<string, decimal> p in results)
        {
            sum += p.Key + ": " + p.Value + ", ";
        }

        INPTRootsText = sum;
        INPTRoots.GetComponent<InputField>().text = INPTRootsText;
    }

    public void ExitTheProgram()
    {
        Application.Quit();
    }
}
