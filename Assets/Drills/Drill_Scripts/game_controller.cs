using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Developed by Brighid Meredith 10/1/2017
 Purpose: controller timer and game logic
 Time is set to initial number of seconds. on time out, game ends. Print time. Print Game Over text. Tally Score.
 Status: Incomplete
     */

public class game_controller : MonoBehaviour {

    public GameObject reliability_reader = null; 
    public bool IsReliabilityDrill = false;
    public float initial_time = 30; //seconds
    public float play_warning_time = 5;
    private float time_remaining;
    private bool game_active = true;
    List<int> incorrect_blocks = new List<int>();
    private string answer_tag = "answer";
    public AudioClip warning_audio;
    public bool game_started = false;
    public bool game_over = false;
    private bool end_game_option = false;
    public GameObject end_game_button;
    Button end_game_button_;
    public GameObject return_to_maingame_button;
    public bool success = false;  // True if game was success, otherwise Fail
    public bool concept_sketch_game = false;

	// Use this for initialization
	void Start () {
        HideAnswers();
        time_remaining = initial_time;
        //initialize and hide end game button
        Text yourButtonText = end_game_button.transform.Find("Text").GetComponent<Text>();
		yourButtonText.text = "End Game";
        end_game_button_ = end_game_button.GetComponent<Button>();
        end_game_button_.onClick.AddListener( () => {game_over = true;} );

        end_game_button.SetActive(false);
	}
	
	// Update is called once per frame
    bool warning_played = false;
	void Update () {
        if(game_started){
            if(!end_game_option){
                //give option to end game early
                end_game_option = true;
                end_game_button.SetActive(true);
            }
            time_remaining -= Time.deltaTime;
            if(time_remaining < play_warning_time && !warning_played){
                warning_played = true;
                AudioSource.PlayClipAtPoint(warning_audio,transform.position);
            }
            if ((time_remaining < 0 && game_active) || game_over)
            {
                game_active = false;
                GameOver();
            }
            else if(game_active)
            {
                gameObject.GetComponent<TextMesh>().text = "Time: "+Mathf.RoundToInt(time_remaining);

            }
        }
    }

    void GameOver()
    {
        //to do list: tally score, display score, display end game animation
        //print("to do list: tally score, display score, display end game animation");
        EndPlay();
        if(!IsReliabilityDrill){
            int score = CalculateScore();
            gameObject.GetComponent<TextMesh>().text = "Score: " + score;
            ShowAnswers();
        }else{
            success = reliability_reader.GetComponent<reliability_calculator>().TargetReached;
            //print("from game_controller..."+success);
            if(success){
                gameObject.GetComponent<TextMesh>().text = "Success!";
            }else{
                gameObject.GetComponent<TextMesh>().text = "Fail...";
            }
        }
        return_to_maingame_button.SetActive(true);

    }

    //hide answers
    void HideAnswers(){
        GameObject[] gos_blocks = GameObject.FindGameObjectsWithTag(answer_tag);
        foreach(GameObject item in gos_blocks){
            item.GetComponent<Renderer>().enabled = false;
            Component[] allChildren = item.GetComponentsInChildren<Transform>();
            foreach (Component child in allChildren) {
                if(child.gameObject.GetComponent<MeshRenderer>()){
                    child.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }

    //show answers if incorrect
    //must be called after CalculateScore() to identify blocks with incorrect answer
    void ShowAnswers(){
        GameObject[] gos_blocks = GameObject.FindGameObjectsWithTag(answer_tag);
        foreach(int block_id in incorrect_blocks){
            for(int j = 0; j < gos_blocks.Length; j++){
                int answer_block_id = gos_blocks[j].GetComponent<drag_drop>().block;
                if(answer_block_id == block_id){
                    gos_blocks[j].GetComponent<Renderer>().enabled = true;
                    Component[] allChildren = gos_blocks[j].GetComponentsInChildren<Transform>();
                    foreach (Component child in allChildren) {
                        if(child.gameObject.GetComponent<MeshRenderer>()){
                            child.gameObject.GetComponent<MeshRenderer>().enabled = true;
                        }
                    }
                }
            }
        }
    }


    //get score
    int CalculateScore(){

        int score = 0;
        if(concept_sketch_game){
            // award points based on concept_sketch_game logic
            GameObject[] gos_blocks = GameObject.FindGameObjectsWithTag("block");
            GameObject[] gos_snapinto = GameObject.FindGameObjectsWithTag("snap_into");
            bool[] snap_into_correct = new bool[gos_snapinto.Length];
            //test each block to determine if within snap_into region
            for(int i = 0; i < gos_blocks.Length; i++){
                bool correct = false;
                Vector3 curr_pos = gos_blocks[i].transform.position;
                for(int j = 0; j < gos_snapinto.Length; j++){
                    List<Vector3> list_of_corners = corners(gos_snapinto[j]);
                    float top_right_x = list_of_corners[0].x;
                    float top_right_y = list_of_corners[0].z;
                    float top_left_x = list_of_corners[1].x;
                    float bottom_left_y = list_of_corners[3].z;
                    if (curr_pos.x < top_right_x && curr_pos.x > top_left_x && curr_pos.z < top_right_y && curr_pos.z > bottom_left_y)
                    {
                        //within this object's domain
                        //test if correct answer
                        int block_id = gos_blocks[i].GetComponent<drag_drop>().block;
                        int snap_id = gos_snapinto[j].GetComponent<drag_drop>().block;
                        if (snap_id == 1){
                            if(block_id == 1){
                                //system correctly assigned
                                score += 1;
                            }else{
                                score = 0;
                            }
                        }
                    }
                }
            }
        }
        else{
            GameObject[] gos_blocks = GameObject.FindGameObjectsWithTag("block");
            GameObject[] gos_snapinto = GameObject.FindGameObjectsWithTag("snap_into");
            bool[] snap_into_correct = new bool[gos_snapinto.Length];
            //test each block to determine if within snap_into region
            for(int i = 0; i < gos_blocks.Length; i++){
                bool correct = false;
                Vector3 curr_pos = gos_blocks[i].transform.position;
                for(int j = 0; j < gos_snapinto.Length; j++){
                    List<Vector3> list_of_corners = corners(gos_snapinto[j]);
                    float top_right_x = list_of_corners[0].x;
                    float top_right_y = list_of_corners[0].z;
                    float top_left_x = list_of_corners[1].x;
                    float bottom_left_y = list_of_corners[3].z;
                    if (curr_pos.x < top_right_x && curr_pos.x > top_left_x && curr_pos.z < top_right_y && curr_pos.z > bottom_left_y)
                    {
                        //within this object's domain
                        //test if correct answer
                        int block_id = gos_blocks[i].GetComponent<drag_drop>().block;
                        int snap_id = gos_snapinto[j].GetComponent<drag_drop>().block;
                        if(block_id == snap_id){
                            //correct answer
                            correct = true;
                            snap_into_correct[j] = true;
                            break;
                        }
                    }
                }
                if(correct){
                    score++;
                }
            }
            //add incorrectly populated snap_into regions to list
            for( int i = 0; i < gos_snapinto.Length; i++){
                if(!snap_into_correct[i]){
                    //this region was not correctly populated
                    int snap_id = gos_snapinto[i].GetComponent<drag_drop>().block;
                    incorrect_blocks.Add(snap_id);
                }
            }
        }
        return score;
        
    }

    //end play--can no longer drag
    void EndPlay(){
        GameObject[] gos_blocks = GameObject.FindGameObjectsWithTag("block");
        for(int i = 0; i < gos_blocks.Length; i++){
            gos_blocks[i].GetComponent<drag_drop>().IsDragable = false;
        }
    }

    //get the coordinates of each corner of gameobject go
    public List<Vector3> corners(GameObject go)
    {
        float width = go.GetComponent<Renderer>().bounds.size.x;
        float height = go.GetComponent<Renderer>().bounds.size.y;
        float depth = go.GetComponent<Renderer>().bounds.size.z;
        Vector3 topRight = go.transform.position, topLeft = go.transform.position, bottomRight = go.transform.position, bottomLeft = go.transform.position;

        topRight.x += width / 2;
        topRight.y += height / 2;
        topRight.z += depth / 2;

        topLeft.x -= width / 2;
        topLeft.y += height / 2;
        topLeft.z += depth / 2;

        bottomRight.x += width / 2;
        bottomRight.y -= height / 2;
        bottomRight.z -= depth / 2;

        bottomLeft.x -= width / 2;
        bottomLeft.y -= height / 2;
        bottomLeft.z -= depth / 2;

        List<Vector3> cor_temp = new List<Vector3>();
        cor_temp.Add(topRight);
        cor_temp.Add(topLeft);
        cor_temp.Add(bottomRight);
        cor_temp.Add(bottomLeft);

        return cor_temp;
    }

}
