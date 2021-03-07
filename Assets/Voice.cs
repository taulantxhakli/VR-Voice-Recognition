/**
 * Voice recognition script, taking in the words and assigning them to
 * the keywords listed below.
 * 
 * @author Taulant Xhakli
 * @version 1.0
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;


public class Voice : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpSpeed = 2.0f;
    public float forceMult = 200;
    public float forceMultSlow = 100;
    public float forceMultRun = 300;
    public float forceMultBack = -100;
    Rigidbody rigid;

    private Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        keywordActions.Add("walk", Walk);
        keywordActions.Add("go", Walk);

        keywordActions.Add("run", Run);
        keywordActions.Add("speed up", Run);

        keywordActions.Add("stop", Stop);

        keywordActions.Add("slow", Slow);
        keywordActions.Add("slow down", Slow);

        keywordActions.Add("turn left", turnLeft);
        keywordActions.Add("turn right", turnRight);

        keywordActions.Add("go up", FloatUp);
        keywordActions.Add("go down", FloatDown);

        keywordActions.Add("home", Home);

        keywordActions.Add("back up", Back);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    private void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Command: " + args.text);
        keywordActions[args.text].Invoke();
    }

    private void Walk()
    {
        rigid.velocity = transform.forward * Time.deltaTime * forceMult;
    }

    private void turnLeft()
    {
        transform.Rotate(0f, -50.0f, 0f);
    }

    private void turnRight()
    {
        transform.Rotate(0f, 50.0f, 0f);
    }

    private void Slow()
    {
        rigid.velocity = transform.forward * Time.deltaTime * forceMultSlow;
    }

    private void Stop()
    {
        rigid.velocity = new Vector3(0, 0, 0);
    }

    private void Run()
    {
        rigid.velocity = transform.forward * Time.deltaTime * forceMultRun;
    }

    private void FloatUp()
    {
        Vector3 jumpVelocity = new Vector3(0f, jumpSpeed, 0f);
        rigid.velocity = rigid.velocity + jumpVelocity; ;
    }

    private void FloatDown()
    {
        Vector3 jumpVelocity = new Vector3(0f, jumpSpeed, 0f);
        rigid.velocity = rigid.velocity - jumpVelocity;
    }

    private void Home()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Back()
    {
        rigid.velocity = transform.forward * Time.deltaTime * forceMultBack;
    }
}