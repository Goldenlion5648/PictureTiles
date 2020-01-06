using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PictureTiles2
{
    class Character
    {

        Texture2D character;
        Rectangle characterRec;

        private int num1, num2, num3;

        private int blockNum;

        private int leftBound;
        private int rightBound;

        private bool isCompleted = false;
        private bool hasBeenPlaced = false;

        private int numBlocksIntersecting;

        private int startingNum;

        private bool isSelected;

        private bool isBlank;

        private int xMovementSpeed, yMovementSpeed;

        private int startingX, startingY;

        private int upperBound, lowerBound;

        private string blockColor = "";

        private bool isActive = false;

        private bool getPos = true;

        private int originalPosition = 0;

        private bool isMovingUp = false;
        private bool isMovingDown = false;

        private bool shouldGetPos = true;

        private bool isMovingLeft;
        private bool isMovingRight;

        public Character(Texture2D c, Rectangle cr)
        {
            character = c;
            characterRec = cr;


        }
        //makes the player jump

        public void moveLeft(int speed)
        {

            characterRec.X -= speed;

        }
        public void moveRight(int speed)
        {

            characterRec.X += speed;

        }
        public void moveUp(int speed)
        {

            characterRec.Y -= speed;

        }
        public void moveDown(int speed)
        {

            characterRec.Y += speed;

        }

        //Patrol behavior
        public void patrol(int leftBoundNum, int rightBoundNum, int speed)
        {
            leftBound = leftBoundNum;
            rightBound = rightBoundNum;


            //when the boolean is true, the position will be stored
            if (getPos == true)
            {
                originalPosition = characterRec.X;
                isMovingRight = true;
                getPos = false;
            }
            //Moves the enemy right
            if (isMovingRight == true)
            {
                characterRec.X += speed;
            }
            //Stops the enemy moving right when it has moved the desired distance
            if (characterRec.X <= leftBound)
            {
                isMovingRight = true;
                isMovingLeft = false;
            }
            //Moves the enemy left
            if (isMovingLeft == true)
            {
                characterRec.X -= speed;
            }
            //Tells the enemy to stop moving left
            if (characterRec.X >= rightBound)
            {
                isMovingLeft = true;
                isMovingRight = false;
            }

        }

        //Patrol behavior
        public void moveBetween(int upperBoundNum, int lowerBoundNum, int speed)
        {
            upperBound = upperBoundNum;
            lowerBound = lowerBoundNum;


            //when the boolean is true, the position will be stored
            if (shouldGetPos == true)
            {
                originalPosition = characterRec.X;
                isMovingDown = true;
                shouldGetPos = false;
            }
            //Moves the enemy right
            if (isMovingDown == true)
            {
                characterRec.Y += speed;
            }
            //Stops the enemy moving down when it has moved the desired distance
            if (characterRec.Y <= upperBound)
            {
                isMovingDown = true;
                isMovingUp = false;
            }
            //Moves the enemy up
            if (isMovingUp == true)
            {
                characterRec.Y -= speed;
            }
            //Tells the enemy to stop moving up
            if (characterRec.Y >= lowerBound)
            {
                isMovingUp = true;
                isMovingDown = false;
            }

        }

        public int GetRektX
        {
            get
            {
                return characterRec.X;
            }
        }

        public int GetRektXLength
        {
            get
            {
                return (characterRec.Right - characterRec.Left);
            }
        }

        public int GetRektYLength
        {
            get
            {
                return (characterRec.Bottom - characterRec.Top);
            }
        }

        public int GetRektY
        {
            get
            {
                return characterRec.Y;
            }
        }

        public Rectangle GetRekt
        {
            get
            {
                return characterRec;
            }
        }

        public void setNum1(int numValue)
        {
            num1 = numValue;
        }
        public void setNum2(int numValue)
        {
            num2 = numValue;
        }
        public void setNum3(int numValue)
        {
            num3 = numValue;
        }

        public void setNumBlocksIntersecting(int newValue)
        {
            numBlocksIntersecting = newValue;
        }

        public int getNumBlocksIntersecting()
        {
            return numBlocksIntersecting;
        }

        public int getXMovementSpeed()
        {
            return this.xMovementSpeed;
        }

        public void setXMovementSpeed(int newSpeed)
        {
            xMovementSpeed = newSpeed;
        }

        public int getYMovementSpeed()
        {
            return this.yMovementSpeed;
        }

        public void setYMovementSpeed(int newSpeed)
        {
            yMovementSpeed = newSpeed;
        }

        public int getNum1()
        {
            return num1;
        }

        public int getNum2()
        {
            return num2;
        }

        public int getNum3()
        {
            return num3;
        }

        public int calcTotal()
        {
            return (num1 + num2 + num3);

        }

        public int calcProduct()
        {
            if (num1 == 0)
            {
                return (num2 * num3);
            }
            else if (num2 == 0)
            {
                return (num1 * num3);
            }
            else if (num3 == 0)
            {
                return (num1 * num2);
            }
            else
            {
                return (num1 * num2 * num3);
            }

        }

        public int calcDifference()
        {
            return (num3 - num2 - num1);

        }

        public bool getIsActive()
        {
            return isActive;

        }

        public void setActive(bool newValue)
        {
            isActive = newValue;

        }

        public int getBlockNum()
        {
            return this.blockNum;

        }

        public int getStartingNum()
        {
            return this.startingNum;

        }

        public void setIsMovingUp(bool newValue)
        {
            isMovingUp = newValue;
        }

        public void setIsMovingDown(bool newValue)
        {
            isMovingDown = newValue;
        }

        public void setIsMovingLeft(bool newValue)
        {
            isMovingLeft = newValue;
        }

        public void setIsMovingRight(bool newValue)
        {
            isMovingRight = newValue;
        }

        public void setShouldGetPos(bool newValue)
        {
            shouldGetPos = newValue;
        }

        public void setGetPos(bool newValue)
        {
            getPos = newValue;
        }

        public void setStartingNum(int newValue)
        {
            startingNum = newValue;

        }

        public void setBlockNum(int newValue)
        {
            blockNum = newValue;

        }
        public void setCompleted(bool newCompletedValue)
        {
            isCompleted = newCompletedValue;
        }

        public void setStartingX(int newStartingX)
        {
            this.startingX = newStartingX;
        }

        public int getStartingX()
        {
            return this.startingX;

        }

        public void setStartingY(int newStartingY)
        {
            this.startingY = newStartingY;
        }

        public int getStartingY()
        {
            return this.startingY;

        }

        public bool getIsCompleted()
        {
            return isCompleted;

        }

        public string getBlockColor()
        {
            return this.blockColor;
        }

        public void setBlockColor(string newColor)
        {
            this.blockColor = newColor;

        }

        public void setX(int position)
        {

            characterRec.X = position;

        }

        public void setY(int position)
        {

            characterRec.Y = position;

        }

        public void setWidth(int newWidth)
        {

            characterRec.Width = newWidth;

        }

        public void setHeight(int newHeight)
        {

            characterRec.Height = newHeight;

        }

        public void setCenter(Rectangle objectToCenterOn)
        {
            characterRec.X = objectToCenterOn.Center.X - characterRec.Width / 2;
            characterRec.Y = objectToCenterOn.Center.Y - characterRec.Height / 2;

        }

        public bool getIsSelected()
        {
            return this.isSelected;
        }

        public void setIsSelected(bool newValue)
        {
            this.isSelected = newValue;
        }

        public bool getIsBlank()
        {
            return this.isBlank;
        }

        public void setIsBlank(bool newValue)
        {
            this.isBlank = newValue;
        }

        public bool getHasBeenPlaced()
        {
            return this.hasBeenPlaced;
        }

        public void setHasBeenPlaced(bool newValue)
        {
            this.hasBeenPlaced = newValue;
        }

        public void changeImage(Texture2D newPic)
        {
            character = newPic;
        }

        public Texture2D getImage()
        {
            return this.character;

        }

        public void DrawCharacter(SpriteBatch sb)
        {

            sb.Draw(character, characterRec, Color.White);

        }

        public void DrawCharacter(SpriteBatch sb, Color color)
        {

            sb.Draw(character, characterRec, color);

        }
    }
}

