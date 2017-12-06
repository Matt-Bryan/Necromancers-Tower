using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * People that touched this script: It's all Will, man
 * A true hero
**/

//written by Will Belden Brown
public class GenerateLevel : MonoBehaviour {
    public int width, height;
    public GameObject[] floorSprites;
    public GameObject[] enemyTier;
    public GameObject exit;
    public GameObject[] objects;
    public GameObject wall;
    public GameObject player;
    public static GenerateLevel instance = null;
    public int numEnemies;

    private Transform levelHolder;
    private Leaf head;
    private List<Leaf> tree = new List<Leaf>();
    private List<Vector3> floorTiles = new List<Vector3>();
    private static int MAX_LEAF_SIZE = 20;

    public void Awake() {
        //ensure only one level generator exists at any given time
        if (!instance)
            instance = this;

        else if (instance != this)
            Destroy(this);
        
        computeLevelStats();
        head = ScriptableObject.CreateInstance<Leaf>().init(-width/2, -height/2, width, height) as Leaf;
        tree.Add(head);
        buildTree();
        head.createRooms();
        buildLevel();
        placeObjectsAtRandom();
    }

    void computeLevelStats() {
        int levelType = Random.Range(0, 12);
        if(levelType == 9) {
            width += height/4;
            height = 3 * height / 4;
        }

        if(levelType == 10) {
            height += width/4;
            width = 3 * width / 4;
        }
    }
    void buildTree() {
        bool didSplit = true;
        buildTree(head, didSplit);
    }

    private void buildTree(Leaf current, bool didSplit) {
        if (current.left == null && current.right == null) {
            if (current.width > MAX_LEAF_SIZE || current.height > MAX_LEAF_SIZE) {
                if(current.split()) {
                    tree.Add(current.left);
                    tree.Add(current.right);
                    buildTree(current.left, didSplit);
                    buildTree(current.right, didSplit);
                    didSplit = true;
                }
            }
        }
    }

    void buildLevel() {
        levelHolder = new GameObject ("Level").transform;
        GameObject toInstantiate;
        foreach(Leaf helper in tree) {
            //place floor tiles within the dimenisions of a leaf's room variable
            if (helper.room != null) {
                for (int x=helper.room.x; x<=(helper.room.x + helper.room.width); x++) {
                    for (int y=helper.room.y; y<=(helper.room.y + helper.room.height); y++) {
                        toInstantiate = floorSprites[Random.Range(0, floorSprites.Length)];
                        GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                        floorTiles.Add(new Vector3(x,y,0));
                        instance.transform.SetParent(levelHolder);
                    }
                }
            }

            //place floor tiles within the dimension of a leaf's hall variables
            else if (helper.halls[0] != null) {
                for (int i=0; i<helper.halls.Length; i++) {
                    if (helper.halls[i]) {
                        for (int x=helper.halls[i].x; x<=(helper.halls[i].x + helper.halls[i].width); x++) {
                            for (int y=helper.halls[i].y; y<=(helper.halls[i].y + helper.halls[i].height); y++) {
                                toInstantiate = floorSprites[Random.Range(0, floorSprites.Length)];
                                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                                floorTiles.Add(new Vector3(x,y,0));
                                instance.transform.SetParent(levelHolder);
                            }
                        }
                    }
                }
            }
        }

        //place wall tiles everywhere there is not a floor tile within the level
        for(int j=-width/2-5; j<=width/2+5; j++) {
            for(int k=-height/2-5; k<=height/2+5; k++) {
                Vector3 pos = new Vector3(j, k, 0);
                if(!floorTiles.Contains(pos)) {
                    // Value never used. Commenting out to remove warning in logs.
                    // GameObject WALL = Instantiate(wall, pos, Quaternion.identity) as GameObject;
                }
            }
        }
    }

    void placeObjectsAtRandom() {
        int numObjects = -1;//width/2 + (Random.Range(-5, 5));
        for(int j=0; j<=numObjects; j++) {
            GameObject instance = Instantiate(objects[Random.Range(0, objects.Length)], floorTiles[Random.Range(0, floorTiles.Count)] - new Vector3(0, 0, 0.3f), Quaternion.identity) as GameObject;
            floorTiles.Remove(instance.transform.position);
        }

        for (int i=0; i<numEnemies; i++) {
            GameObject enemy = Instantiate(enemyTier[Random.Range(0, enemyTier.Length)], floorTiles[Random.Range(0, floorTiles.Count)] - new Vector3(0, 0, 0.3f), Quaternion.identity) as GameObject;
            floorTiles.Remove(enemy.transform.position);
        }

        // Value never used. Commenting out to remove warning in logs.
        // GameObject EXIT = Instantiate(exit, floorTiles[Random.Range(0, floorTiles.Count)] - new Vector3(0, 0, 0.3f), Quaternion.identity) as GameObject;

        player.transform.position = floorTiles[Random.Range(0, floorTiles.Count)];
    }
	
	// Update is called once per frame
	void Update () {
	}
}
