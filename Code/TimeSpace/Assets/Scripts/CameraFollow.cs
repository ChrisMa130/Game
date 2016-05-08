using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public float xMargin = 1f;      // Distance in the x axis the player can move before the camera follows.
    public float yMargin = 1f;      // Distance in the y axis the player can move before the camera follows.
    public float xSmooth = 8f;      // How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 8f;      // How smoothly the camera catches up with it's target movement in the y axis.
    public Vector2 maxXAndY;        // The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY;        // The minimum x and y coordinates the camera can have.

    private float viewportHeight;
    private float viewportWidth;

    // Use this for initialization
    public Transform player;
    public tk2dTileMap map;

    void Start()
    {
        //计算摄像机边距
        UpdateViewportSize();
    }

    private void UpdateViewportSize()
    {
        //计算地图范围
        Vector3 minMap= map.GetTilePosition(0, 0);
        Vector3 maxMap = map.GetTilePosition(map.width, map.height);

        
        



        Camera cam = GetComponent<Camera>();
        print(cam.aspect);
        //print(cam.pixelHeight + " " + Screen.height);
        //print(1 + 2 + "" + 3 + 5);
        Vector3 minp = cam.ViewportToWorldPoint(new Vector3(0, 0));
        print(minp);
        Vector3 maxp = cam.ViewportToWorldPoint(new Vector3(1, 1));
        print(maxp);
        viewportWidth = Mathf.Abs(minp.x - maxp.x);
        viewportHeight = Mathf.Abs(minp.y - maxp.y);

        print(viewportWidth + " " + viewportHeight);

        maxXAndY =new Vector2(maxMap.x - viewportWidth / 2, maxMap.y - viewportHeight / 2);
        minXAndY = new Vector2(minMap.x + viewportWidth / 2, minMap.y + viewportHeight / 2);

    }
    public void FixedUpdate()
    {
        //transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        TrackPlayer();
        
    }

    // Update is called once per frame
    void Update()
    {
        //float targetY = Mathf.Lerp(transform.position.y, player.position.y, xSmooth * Time.deltaTime);
        //float targetX = Mathf.Lerp(transform.position.x, player.position.x, ySmooth * Time.deltaTime);
        //transform.position = new Vector3(targetX, targetY, transform.position.z);
        
    }


    bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }


    bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }

    void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        // If the player has moved beyond the x margin...
        if (CheckXMargin())
            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

        // If the player has moved beyond the y margin...
        if (CheckYMargin())
            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
