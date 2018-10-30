using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    [SerializeField] private float ballspeed = 15f;
	[SerializeField] private float paddleSpeed = 10f;
	[SerializeField] private Transform LPaddle;
    [SerializeField] private Transform RPaddle;
    [SerializeField] private Text LScore;
    [SerializeField] private Text RScore;
    
    
    float verticalExtent;
    float horizontalExtent;
	float paddleYRadius;
	float ballRadius;
    int lScore, rScore;

    Vector3 dir;
    Vector3 moveVertical, moveVertical1;
    Vector3 ballPosition;

	// Use this for initialization
	void Start () {
        verticalExtent = Camera.main.orthographicSize;
        horizontalExtent = verticalExtent * Camera.main.aspect;

		LPaddle.position = new Vector3(-horizontalExtent + 0.8f, 0f, 0f);
		RPaddle.position = new Vector3(horizontalExtent - 0.8f, 0f, 0f);
		paddleYRadius = LPaddle.transform.localScale.y / 2;
		ballRadius = this.transform.localScale.x / 2;

        BallReset();    //Intializing position to zero and direction of Ball 

    }
	
	// Update is called once per frame
	void Update () {

		BallMovement();
		CheckCollisionsWithPaddles();
		
		if (Input.GetKey(KeyCode.Space))    //Reset Ball for every Space Key
            BallReset();

        PaddleMovements(); //Movement for Paddles
    }

	void BallMovement()
	{
		this.transform.position += dir * ballspeed * Time.deltaTime;	//BallMovement

		/*Clamp Ball to stay inside the camera Vertically*/
		if (this.transform.position.y > verticalExtent)
			dir.y *= -1;
		if (this.transform.position.y < -verticalExtent)
			dir.y *= -1;
	}

	void BallReset()    //Reset Ball
	{
		this.transform.position = Vector2.zero;
		dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
	}

	void PaddleMovements()  //Movement for Paddles
    {
        moveVertical = new Vector3(0f, Input.GetAxis("Vertical"), 0f);
        LPaddle.transform.position += moveVertical * paddleSpeed * Time.deltaTime;
        moveVertical1 = new Vector3(0f, Input.GetAxis("Vertical1"), 0f);
        RPaddle.transform.position += moveVertical1 * paddleSpeed * Time.deltaTime;

		/*Clamp Paddles to stay inside frame*/
		var pos = LPaddle.transform.position;
		pos.y = Mathf.Clamp(pos.y, -verticalExtent + paddleYRadius, verticalExtent - paddleYRadius);
		LPaddle.transform.position = pos;

		pos = RPaddle.transform.position;
		pos.y = Mathf.Clamp(pos.y, -verticalExtent + paddleYRadius, verticalExtent - paddleYRadius);
		RPaddle.transform.position = pos;
		/*End of Clamp*/
		
    }

	void CheckCollisionsWithPaddles()
	{
        //Check Collision with LeftPaddle
        if (this.transform.position.x - ballRadius < LPaddle.position.x)
        {
            if (this.transform.position.y < LPaddle.position.y + paddleYRadius && this.transform.position.y > LPaddle.position.y - paddleYRadius)
                dir.x *= -1;
            else
            {
                rScore++;
                RScore.text = rScore.ToString();
                BallReset();
            }
        }
        //Check Collision with RightPaddle
        if (this.transform.position.x + ballRadius > RPaddle.position.x)
        {
            if (this.transform.position.y < RPaddle.position.y + paddleYRadius && this.transform.position.y > RPaddle.position.y - paddleYRadius)
                dir.x *= -1;
            else
            {
                lScore++;
                LScore.text = lScore.ToString();
                BallReset();
            }
        }
	}
}
