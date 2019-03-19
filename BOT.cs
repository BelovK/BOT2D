using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOT : MonoBehaviour
{
    private RaycastHit2D ForvardRH,LeftRH,RightRH, Left2RH, Right2RH, Left3RH, Right3RH;
    public Transform Target;
    public float len = 100;
    private NeuralNetwork Net;
    private Rigidbody2D RB;
    private bool initilized = false;

    private float[] output;

    private Vector3 OldPos;

    private float TravelDistance = 0;
    private float TargetDistance;

    private float fitness = 0;

    private Transform camCL,camCR,camC,camBL,camBR,camR,camL;

    private SpriteRenderer SR;
    private float speed = 6f;
    private float startdistace;

    private LineRenderer LR;
    // Use this for initialization
    void Start ()
    {
        RB = GetComponent<Rigidbody2D>();
        TargetDistance = Vector3.Distance(transform.position, Target.position);
        startdistace = TargetDistance;
        OldPos = transform.position;
        camCL = transform.Find("CamCL").GetComponent<Transform>();
        camCR = transform.Find("CamCR").GetComponent<Transform>();
        camC = transform.Find("Cam").GetComponent<Transform>();
        camL = transform.Find("CamL").GetComponent<Transform>();
        camR = transform.Find("CamR").GetComponent<Transform>();
        camBL = transform.Find("CamBL").GetComponent<Transform>();
        camBR = transform.Find("CamBR").GetComponent<Transform>();
        SR = GetComponent<SpriteRenderer>();
        LR = GetComponent<LineRenderer>();
        //Invoke("Timer", 0.5f);
    }

    void Timer()
    {
        if(initilized)
            LR.enabled = true;
    }

    // Update is called once per frame
        void FixedUpdate()
    {
        if (initilized == true)
        {
            
            Vector3 LeftCam = camCL.right;
            Vector3 RightCam = camCR.right;
            Vector3 Left2Cam = camBL.right;
            Vector3 Right2Cam = camBR.right;
            Vector3 Left3Cam = camL.right;
            Vector3 Right3Cam = camR.right;
            ForvardRH = Physics2D.Raycast(camC.position,
                transform.right * len, len);
            LeftRH = Physics2D.Raycast(camCL.position, LeftCam * len, len);
            RightRH = Physics2D.Raycast(camCR.position, RightCam * len, len);
            Left2RH = Physics2D.Raycast(camBL.position, Left2Cam * len, len);
            Right2RH = Physics2D.Raycast(camBR.position, Right2Cam * len, len);
            Left3RH = Physics2D.Raycast(camL.position, Left3Cam * len, len);
            Right3RH = Physics2D.Raycast(camR.position, Right3Cam * len, len);
            if (SR.color == Color.green)
            {
                LR.enabled = true;
                for (int i = 0; i < LR.positionCount; i++)
                {
                    LR.SetPosition(i,transform.position);
                }
                //DrawRay(ForvardRH, Color.green);
                //Debug.DrawLine(transform.position, ForvardRH.point, Color.green);
                LR.startColor = LR.endColor = Color.green;
                LR.SetPosition(0,transform.position);
                LR.SetPosition(1, ForvardRH.point);
                LR.SetPosition(2, transform.position);
                LR.SetPosition(3, LeftRH.point);
                LR.SetPosition(4, transform.position);
                LR.SetPosition(5, RightRH.point);
                LR.SetPosition(6, transform.position);
                LR.SetPosition(7, Left2RH.point);
                LR.SetPosition(8, transform.position);
                LR.SetPosition(9, Right2RH.point);
                LR.SetPosition(12, transform.position);
                LR.SetPosition(13, Left3RH.point);
                LR.SetPosition(10, transform.position);
                LR.SetPosition(11, Right3RH.point);
            }
            else
            {
                LR.enabled = false;
            }

            float[] inputs = new float[7];
            inputs[0] = ForvardRH.distance;
            inputs[1] = LeftRH.distance;
            inputs[2] = RightRH.distance;
            inputs[3] = Left2RH.distance;
            inputs[4] = Right2RH.distance;
            inputs[5] = Left3RH.distance;
            inputs[6] = Right3RH.distance;
            output = Net.FeedForward(inputs);
            RB.velocity = speed * transform.right;
            RB.angularVelocity = 500f * output[0];
            TravelDistance += Vector3.Distance(transform.position, OldPos);
            OldPos = transform.position;
            TargetDistance = Vector3.Distance(transform.position, Target.position);
            fitness = (1 / TargetDistance) * 10 - TravelDistance / (startdistace * 10);
            //fitness = (1 / TargetDistance) * 10;
            Net.SetFitness(fitness);
            if (TargetDistance < 0.6f)
            {
                Net.AddFitness(10f);
                LR.enabled = false;
                initilized = false;
            }
            
        }
        else
        {
            LR.enabled = false;
            RB.velocity = Vector2.zero;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Box")
        {
            //Debug.Log("done"+gameObject.GetInstanceID());
            initilized = false;
            GetComponent<SpriteRenderer>().enabled = false;
            SpriteRenderer[] sr= GetComponentsInChildren<SpriteRenderer>();
            foreach (var s in sr)
            {
                s.color = new Color(1,1,1,0.1f);
                //s.enabled = false;
            }
        }
    }
    void DrawRay(RaycastHit2D RH,Color c)
    {
        if (RH.collider != null)
        {
            Debug.DrawLine(transform.position, RH.point, c);
        }
    }

    public RaycastHit2D GetFR()
    {
        return ForvardRH;
    }
    public RaycastHit2D GetLR()
    {
        return LeftRH;
    }
    public RaycastHit2D GetRR()
    {
        return RightRH;
    }

    public float GetFitness()
    {
        return fitness;
    }
    public float GetOut()
    {
        if(output != null)
            return output[0];
        else
        {
            return 0;
        }
    }
    public void Init(NeuralNetwork net, Transform hex)
    {
        this.Target = hex;
        this.Net = net;
        initilized = true;
    }

    public bool GetStatus()
    {
        return initilized;
    }
}
