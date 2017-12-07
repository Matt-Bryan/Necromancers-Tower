using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that touched this script: Will Belden Brown
**/

//written by Will Belden Brown
public class Leaf : ScriptableObject {
    private const int MIN_LEAF_SIZE = 6;

    public int x, y, width, height;
    public Leaf left, right = null;
    public Rectangle room = null;
    public Rectangle[] halls = new Rectangle[2];

    public Leaf init (int X, int Y, int Width, int Height) {
        x = X;
        y = Y;
        width = Width;
        height = Height;
        halls[0] = null;
        halls[1] = null;
        return this;
    }

    public bool split() {
        if (left != null || right != null)
            return false;

        bool splitH = Random.Range(0,1) == 0;
        if(height > width && (float)height/(float)width >= 1.25)
            splitH = true;

        else if(width > height && (float)width/(float)height >= 1.25)
            splitH = false;

        int max = (splitH ? height : width) - MIN_LEAF_SIZE;
        if (max <= MIN_LEAF_SIZE)
            return false;

        int split = Random.Range(MIN_LEAF_SIZE, max);
        if (splitH) {
            left = CreateInstance<Leaf>().init(x, y, width, split) as Leaf; //new Leaf(x, y, width, split);
            right = CreateInstance<Leaf>().init(x, y + split, width, height - split) as Leaf; //new Leaf(x, y + split, width, height - split);
        }

        else {
            left = CreateInstance<Leaf>().init(x, y, split, height) as Leaf;//new Leaf(x, y, split, height);
            right = CreateInstance<Leaf>().init(x+split, y, width-split, height) as Leaf;//new Leaf(x + split, y, width-split, height);
        }

        return true;
    }

    public void createRooms() {

        if (left || right) {
            if(left)
                left.createRooms();

            if(right)
                right.createRooms();

            if(left && right) {
                createHall(left.getRoom(), right.getRoom());
            }   
        }

        else {
            Vector2 roomSize;
            Vector2 roomPosition;

            roomSize =  new Vector2(Random.Range(3, width-2), Random.Range(3, height-2));
            roomPosition = new Vector2(Random.Range(1, width - roomSize.x - 1), Random.Range(1, height - roomSize.y - 1));
            room = CreateInstance<Rectangle>().init(x + (int)roomPosition.x, y + (int)roomPosition.y, (int)roomSize.x, (int)roomSize.y) as Rectangle;
        }
    }

    public Rectangle getRoom() {
        if(room)
            return room;

        else {
            if (left.getRoom() == null && right.getRoom() == null)
                return null;

            else if (!left.getRoom())
                return right.getRoom();

            else if (!right.getRoom())
                return left.getRoom();

            else if (Random.Range(0,1) > 0)
                return left.getRoom();

            else
                return right.getRoom();
        }
    }

    public void createHall(Rectangle Left, Rectangle Right) {
        if (!Left || !Right)
            return;

        Vector2 pt1 = new Vector2(Random.Range(Left.x +1, Left.x + Left.width -2), Random.Range(Left.y +1, Left.y + Left.height -2));
        Vector2 pt2 = new Vector2(Random.Range(Right.x +1, Right.x + Right.width -2), Random.Range(Right.y +1, Right.y + Right.height -2));
    
        int w = (int)pt2.x - (int)pt1.x;
        int h = (int)pt2.y - (int)pt1.y;

        if (w < 0) {
            if (h < 0) {
                if (Random.Range(0,1) > 0) {
                    halls[0] = CreateInstance<Rectangle>().init((int)pt2.x, (int)pt1.y, Mathf.Abs(w), 1) as Rectangle;
                    halls[1] = CreateInstance<Rectangle>().init((int)pt2.x, (int)pt2.y, 1, Mathf.Abs(h)) as Rectangle;
                }

                else {
                    halls[0] = CreateInstance<Rectangle>().init((int)pt2.x, (int)pt2.y, Mathf.Abs(w), 1) as Rectangle;
                    halls[1] = CreateInstance<Rectangle>().init((int)pt1.x, (int)pt2.y, 1, Mathf.Abs(h)) as Rectangle;
                }
            }

            else if (h > 0) {
                if (Random.Range(0,1) > 0) {
                    halls[0] = CreateInstance<Rectangle>().init((int)pt2.x, (int)pt1.y, Mathf.Abs(w), 1) as Rectangle;
                    halls[1] = CreateInstance<Rectangle>().init((int)pt2.x, (int)pt1.y, 1, Mathf.Abs(h)) as Rectangle;
                }

                else {
                    halls[0] = CreateInstance<Rectangle>().init((int)pt2.x, (int)pt2.y, Mathf.Abs(w), 1) as Rectangle;
                    halls[1] = CreateInstance<Rectangle>().init((int)pt1.x, (int)pt1.y, 1, Mathf.Abs(h)) as Rectangle;
                }
            }

            else
                halls[0] = CreateInstance<Rectangle>().init((int)pt2.x, (int)pt2.y, Mathf.Abs(w), 1) as Rectangle;
        }

        else if (w > 0) {
             if (h < 0) {
                if (Random.Range(0,1) > 0) {
                    halls[0] = CreateInstance<Rectangle>().init((int)pt1.x, (int)pt2.y, Mathf.Abs(w), 1) as Rectangle;
                    halls[1] = CreateInstance<Rectangle>().init((int)pt1.x, (int)pt2.y, 1, Mathf.Abs(h)) as Rectangle;
                }

                else {
                    halls[0] = CreateInstance<Rectangle>().init((int)pt1.x, (int)pt1.y, Mathf.Abs(w), 1) as Rectangle;
                    halls[1] = CreateInstance<Rectangle>().init((int)pt2.x, (int)pt2.y, 1, Mathf.Abs(h)) as Rectangle;
                }
            }

            else if (h > 0) {
                if (Random.Range(0,1) > 0) {
                    halls[0] = CreateInstance<Rectangle>().init((int)pt1.x, (int)pt1.y, Mathf.Abs(w), 1) as Rectangle;
                    halls[1] = CreateInstance<Rectangle>().init((int)pt2.x, (int)pt1.y, 1, Mathf.Abs(h)) as Rectangle;
                }

                else {
                    halls[0] = CreateInstance<Rectangle>().init((int)pt1.x, (int)pt2.y, Mathf.Abs(w), 1) as Rectangle;
                    halls[1] = CreateInstance<Rectangle>().init((int)pt1.x, (int)pt1.y, 1, Mathf.Abs(h)) as Rectangle;
                }
            }

            else
                halls[0] = CreateInstance<Rectangle>().init((int)pt1.x, (int)pt1.y, Mathf.Abs(w), 1) as Rectangle;
      
        }

        else {
            if (h < 0)
                halls[0] = CreateInstance<Rectangle>().init((int)pt2.x, (int)pt2.y, 1, Mathf.Abs(h)) as Rectangle;
            
            else if (h >= 0)
                halls[0] = CreateInstance<Rectangle>().init((int)pt1.x, (int)pt1.y, 1, Mathf.Abs(h)) as Rectangle;
        }
    }
}