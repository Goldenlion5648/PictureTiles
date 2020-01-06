using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using xna = Microsoft.Xna.Framework;
using drawing = System.Drawing;

namespace PictureTiles2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : xna.Game
    {
        #region Objects
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouseState, oldmouseState;
        KeyboardState kb, oldkb;

        List<Character> listOfCharacters;
        Character[,] gridOfCharacters;

        //Character imageToSplit;

        Character playButton;
        Character optionsButton;
        //Character colorChoiceButton;
        //Character timerLengthChoiceButton;
        //Character enableTimedModeButton;
        //Character livesButton;
        Character goBackButton;
        Character goToTitleScreenButton;
        Character colorChoiceDisplay;
        Character puzzleChoiceButton;

        Character difficultyButton;

        //Character tempCharacter;

        Character selectionIndicator;
        Character animationCharacter1, animationCharacter2, animationCharacter3;

        Character puzzleChoicePreview;

        Bitmap imageInGrid1, exportedImage, croppedImage, tempPic1, tempPic2;

        List<Bitmap> imageParts;

        xna.Rectangle[] recImageArray;



        #endregion

        #region variables
        bool startButtonSelected, optionsButtonSelected, goBackButtonSelected, goToTitleScreenButtonSelected, puzzleChoiceButtonSelected, difficultyButtonSelected;
        xna.Color colorChoice = xna.Color.LightBlue;
        int colorChoiceNum = 2;



        //string importPath = "C:\\Users\\Kay\\Documents\\C# Games\\PictureTiles\\PictureTiles\\PictureTilesContent\\";
        //string exportPath = "C:\\Users\\Kay\\Documents\\C# Games\\PictureTiles\\PictureTiles\\PictureTilesContent\\TilesForGrid\\";



        string importPath = Path.GetFullPath(@"PictureTilesContent");
        string exportPath = Path.GetFullPath(@"PictureTilesContent");


        string path;
        string newPath;

        string desktopPath;

        //System.IO.Directory.GetParent();

        //string test = Path.GetDirectoryName("PictureTilesContent");
        //string test2 = Path.GetDirectoryName("TilesForGrid");



        //change to change the file name for the image imgSQI or imgCN (normal code ninja) or imgCNSmall, imgCoding
        //********************MUST CHANGE LOAD CONTENT IMAGE ALSO*****************
        //image to load


        //2019 Note: Use the image split program and just replace this line with the image name

        string imagePrefixForFile = "blobsBorder";
        //SquareIconResized || cnLogoResized
        string originalFileToLoad = "blobsBorder";
        //when adding a new image, change this to true
        bool shouldTryCreatingNewImages = false;


        int totalNumOfPuzzles = 3;

        int puzzleChoiceNum = 1;

        int difficultyNum = 5;

        //int numPicturesCreated = Settings1.Default.numCreatedPictures;
        bool hasDoneStartOfGameCode = false;

        Texture2D whiteSquare, lines;

        Texture2D[] previewImages;

        bool hasMoved = false;
        bool hasSelected = false;

        bool isSelectionLocked = false;
        bool isSelectorIncreasing;
        int selectorPulsarCounter;

        int gridPosCounter;

        int delayTimer = 350;

        int counterForLoops = 0;

        int randomSwapper1, randomSwapper2;
        int previousSwapNum;

        //int difficultyMultiplier = 6;

        int animationStartTime, animationEndTime;
        bool shouldGetAnimationStartTime = true;
        bool isAnimationActive = false;
        bool isAnimationVertical = false;
        bool isAnimationGoingUp = false;
        bool isAnimationGoingLeft = false;

        bool shouldTryCheckingAnswer = false;

        bool isAnswerCorrect = false;

        bool isGameOver = false;

        Texture2D tempAnimationImage1, tempAnimationImage2, tempAnimationImage3, tempSwapImage;

        int gameClock;

        int imgCounter = 0;

        int xTimesToDivideImage = 4;
        int yTimesToDivideImage = 3;
        Texture2D[] imagePartsAsTex2D, imagePartsAsTex2DScrambled;
        int imageSizeX, imageSizeY;

        string stateOfTimerMode = "Off";

        Random rand = new Random();

        int screenW, screenH;

        SpriteFont customfont, titleAndEndFont, buttonFont;
        #endregion

        #region gamestateThings

        enum gameState
        {

            titleScreen, gamePlay, endScreen, Lose, options, Win

        }

        gameState state = gameState.titleScreen;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Sets graphics to 1080p
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            //Shows cursor, is optional
            IsMouseVisible = true;
            //Acts weird if this isn't added
            this.Window.AllowUserResizing = true;







        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Texture2D[] imagePartsAsTexture2D = new Texture2D[xTimesToDivideImage * yTimesToDivideImage];
            imagePartsAsTex2D = new Texture2D[xTimesToDivideImage * yTimesToDivideImage];
            imagePartsAsTex2DScrambled = new Texture2D[xTimesToDivideImage * yTimesToDivideImage];

            listOfCharacters = new List<Character>(xTimesToDivideImage * yTimesToDivideImage);

            recImageArray = new xna.Rectangle[xTimesToDivideImage * yTimesToDivideImage];

            previewImages = new Texture2D[totalNumOfPuzzles];

            desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\PictureTilesContent\\";

            path = importPath;
            newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\..\"));
            Console.WriteLine(newPath);


            importPath = desktopPath;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenH = GraphicsDevice.Viewport.Height;
            screenW = GraphicsDevice.Viewport.Width;

            customfont = Content.Load<SpriteFont>("customfont");
            titleAndEndFont = Content.Load<SpriteFont>("titleAndEndFont");
            buttonFont = Content.Load<SpriteFont>("buttonFont");

            //title screen buttons
            playButton = new Character(Content.Load<Texture2D>("newPlayButton2"), new xna.Rectangle(screenW / 2 - 76, screenH / 2 - 25, 153, 60));
            optionsButton = new Character(Content.Load<Texture2D>("optionsButton2"), new xna.Rectangle(screenW / 2 - 76, screenH / 2 + 45, 153, 60));
            //colorChoiceButton = new Character(Content.Load<Texture2D>("buttonOutline"), new xna.Rectangle(screenW / 2 - 76, screenH / 2 - 95, 153, 60));

            puzzleChoiceButton = new Character(Content.Load<Texture2D>("buttonOutline"), new xna.Rectangle(screenW / 2 - 76, screenH / 2 - 195, 153, 60));
            difficultyButton = new Character(Content.Load<Texture2D>("buttonOutline"), new xna.Rectangle(screenW / 2 - 76, screenH / 2 - 95, 153, 60));


            colorChoiceDisplay = new Character(Content.Load<Texture2D>("WhiteSquare"), new xna.Rectangle(screenW / 2 + 126, screenH / 2 - 95, 50, 50));

            //timerLengthChoiceButton = new Character(Content.Load<Texture2D>("buttonOutline"), new xna.Rectangle(screenW / 2 - 7, screenH / 2 - 25, 193, 60));
            //timerLengthChoiceButton = new Character(Content.Load<Texture2D>("buttonOutline"), new xna.Rectangle(screenW / 2 - ((timerLengthChoiceButton.GetRektXLength) / 2),
            //    screenH / 2 - 25, 193, 60));
            //livesButton = new Character(Content.Load<Texture2D>("buttonOutline"), new xna.Rectangle(screenW / 2 - 7, screenH / 2 + 115, 193, 60));
            //livesButton = new Character(Content.Load<Texture2D>("buttonOutline"), new xna.Rectangle(screenW / 2 - ((timerLengthChoiceButton.GetRektXLength) / 2),
            //    screenH / 2 + 115, 193, 60));
            //enableTimedModeButton = new Character(Content.Load<Texture2D>("buttonOutline"), new xna.Rectangle(screenW / 2 - 76, screenH / 2 + 45, 153, 60));

            goToTitleScreenButton = new Character(Content.Load<Texture2D>("buttonOutline"), new xna.Rectangle(screenW / 2 - 136, screenH / 2 + 45, 272, 60));

            goBackButton = new Character(Content.Load<Texture2D>("buttonOutline"), new xna.Rectangle(40, screenH - 75, 153, 60));

            selectionIndicator = new Character(Content.Load<Texture2D>("SelectionIndicatorBlue"), new xna.Rectangle(200, 0, screenW / xTimesToDivideImage, screenH / yTimesToDivideImage));
            animationCharacter1 = new Character(Content.Load<Texture2D>("SelectionIndicatorBlue"), new xna.Rectangle(200, 0, screenW / xTimesToDivideImage, screenH / yTimesToDivideImage));
            animationCharacter2 = new Character(Content.Load<Texture2D>("SelectionIndicatorBlue"), new xna.Rectangle(200, 0, screenW / xTimesToDivideImage, screenH / yTimesToDivideImage));
            animationCharacter3 = new Character(Content.Load<Texture2D>("SelectionIndicatorBlue"), new xna.Rectangle(200, 0, screenW / xTimesToDivideImage, screenH / yTimesToDivideImage));

            puzzleChoicePreview = new Character(Content.Load<Texture2D>("blobsBorder"), new xna.Rectangle(screenW / 2 + 90, screenH / 2 - 195, 80, 60));

            previewImages[0] = Content.Load<Texture2D>("blobsBorder");
            previewImages[1] = Content.Load<Texture2D>("squareIconResized");
            previewImages[2] = Content.Load<Texture2D>("CodingWords");





            //imageInGrid1 = new Bitmap(Image.FromFile(importPath + "cnLogoResized.png"));

            //imageInGrid1 = new Bitmap(Image.FromFile(importPath + originalFileToLoad + ".png"));




            //test1 = Content.Load<Texture2D>("imageInGrid1");

            gridOfCharacters = new Character[xTimesToDivideImage, yTimesToDivideImage];


            imageParts = new List<Bitmap>(xTimesToDivideImage * yTimesToDivideImage);

            imagePartsAsTex2D[0] = Content.Load<Texture2D>("img1");

            whiteSquare = Content.Load<Texture2D>("WhiteSquare");
            lines = Content.Load<Texture2D>("BlackSquare");
            tempAnimationImage1 = whiteSquare;
            tempAnimationImage2 = whiteSquare;
            tempAnimationImage3 = whiteSquare;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            IsMouseVisible = true;
            mouseState = Mouse.GetState();
            kb = Keyboard.GetState();

            switch (state)
            {

                case gameState.titleScreen:
                    titleScreen();
                    break;
                case gameState.gamePlay:

                    gamePlay(gameTime);
                    break;
                case gameState.Lose:

                    Lose();
                    break;
                case gameState.Win:

                    Win();
                    break;
                case gameState.endScreen:

                    endScreen();
                    break;
                case gameState.options:

                    optionsScreen();
                    break;

            }

            oldmouseState = mouseState;
            oldkb = kb;
            base.Update(gameTime);
        }

        private void cropImage(Bitmap img, drawing.Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            croppedImage = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

        }

        private void cropImageAndSave(Bitmap img, drawing.Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            croppedImage = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

            //croppedImage.Save(exportPath + @"cropped_pic" + Settings1.Default.numCreatedPictures.ToString() + ".png");
            croppedImage.Save(exportPath + @"cropped_pic" + ".png");


        }

        private void makeAlteredImage(Bitmap imageToChange)
        {
            exportedImage = imageToChange;
            //go through the pixels 
            for (int x = 0; x < exportedImage.Width; x++)
            {
                for (int y = 0; y < exportedImage.Height; y++)
                {
                    drawing.Color oldColor = exportedImage.GetPixel(x, y);
                    drawing.Color newColor = drawing.Color.FromArgb(0, 0, oldColor.B);
                    if (newColor.R + newColor.G + newColor.B == 0)
                    {
                        newColor = drawing.Color.DarkOrchid;
                    }
                    exportedImage.SetPixel(x, y, newColor);

                }

            }

            //exportedImage.Save(exportPath + @"newPic" + Settings1.Default.numCreatedPictures.ToString() + ".png");

            //Settings1.Default.numCreatedPictures += 1;
            //Settings1.Default.Save();

        }

        private void startOfGameCode()
        {
            if (hasDoneStartOfGameCode == false)
            {
                //changes definition of imageInGrid1
                //makeAlteredImage(imageInGrid1);
                //say area to crop
                //cropImageAndSave(imageInGrid1, new drawing.Rectangle(160, 5, 578, 451));
                isGameOver = false;

                getImageParts(imageInGrid1);
                //getImageParts(imageInGrid2);
                resetGridForStartOfGame();

                shouldTryCheckingAnswer = false;
                delayTimer = 350;
                isSelectionLocked = false;

                setGridToDefault();


                //scrambleGrid();

                hasDoneStartOfGameCode = true;
            }

        }

        private void setGridToDefault()
        {

            counterForLoops = 0;
            gridOfCharacters[0, 0].changeImage(whiteSquare);
            for (int y = 0; y < gridOfCharacters.GetLength(1); y += 1)
            {
                for (int x = 0; x < gridOfCharacters.GetLength(0); x += 1)
                {
                    gridOfCharacters[x, y].changeImage(imagePartsAsTex2D[counterForLoops]);

                    counterForLoops++;
                }


            }

            imagePartsAsTex2D[0] = whiteSquare;
            imagePartsAsTex2DScrambled[0] = whiteSquare;

            Console.WriteLine("grid set to default");
            gridOfCharacters[0, 0].changeImage(whiteSquare);


        }

        private void checkAnswer()
        {
            if (gameClock % 20 == 0 && shouldTryCheckingAnswer == true)
            {
                isAnswerCorrect = true;
                gridPosCounter = 0;
                for (int y = 0; y < gridOfCharacters.GetLength(1); y++)
                {
                    for (int x = 0; x < gridOfCharacters.GetLength(0); x++)
                    {
                        if (isAnswerCorrect == true)
                        {

                            if (gridOfCharacters[x, y].getImage() != imagePartsAsTex2D[gridPosCounter])
                            {
                                //Console.WriteLine("gridOfCharacters " + x + ", " + y + " did not equal imagePartsAsTex2D[" + gridPosCounter);

                                isAnswerCorrect = false;
                            }
                            else if (gridOfCharacters[0, 0].getImage() == whiteSquare)
                            {

                                isAnswerCorrect = true;
                            }
                            else
                            {
                                //Console.WriteLine("gridOfCharacters " + x + ", " + y + " **DOES** equal imagePartsAsTex2D[" + gridPosCounter);
                            }


                        }

                        gridPosCounter++;
                    }


                }

                //if (isAnswerCorrect == false)
                //{
                //    isAnswerCorrect = true;
                //}
                //for (int i = 0; i < imagePartsAsTex2DScrambled.Length; i++)
                //{
                //    if (isAnswerCorrect == true && imagePartsAsTex2D[i] != imagePartsAsTex2DScrambled[i])
                //    {
                //        isAnswerCorrect = false;
                //        Console.WriteLine("imagePartsAsTex2D 1: " + imagePartsAsTex2D[i]);
                //        Console.WriteLine("imagePartsAsTex2DScrambled 2: " + imagePartsAsTex2DScrambled[i]);
                //        Console.WriteLine("were not matching");

                //    }

                //}


                if (isAnswerCorrect == true)
                {
                    isGameOver = true;
                    imagePartsAsTex2D[0] = Content.Load<Texture2D>("TilesForGrid/" + imagePrefixForFile + 0);
                    imagePartsAsTex2DScrambled[0] = Content.Load<Texture2D>("TilesForGrid/" + imagePrefixForFile + 0);
                    gridOfCharacters[0, 0].changeImage(imagePartsAsTex2D[0]);
                    //Console.WriteLine("answers correct");
                    shouldTryCheckingAnswer = false;

                }
                else
                {
                    //Console.WriteLine("answers incorrect");
                }

            }


        }

        private void scrambleGrid()
        {
            //isGameOver = false;

            setGridToDefault();



            for (int i = 0; i <= difficultyNum * 2; i++)
            {

                for (int y = 0; y < gridOfCharacters.GetLength(1); y++)
                {
                    //nested for loop for 2nd dimension
                    for (int x = 0; x < gridOfCharacters.GetLength(0); x++)
                    {
                        if (gridOfCharacters[x, y].getImage() == whiteSquare)
                        {
                            while (gridOfCharacters[x, y].getImage() == whiteSquare)
                            {
                                randomSwapper1 = rand.Next(1, 5);
                                if (randomSwapper1 == 1 && x + 1 < gridOfCharacters.GetLength(0) && previousSwapNum != 2)
                                {
                                    previousSwapNum = 1;
                                    tempSwapImage = gridOfCharacters[x + 1, y].getImage();
                                    gridOfCharacters[x + 1, y].changeImage(whiteSquare);
                                    gridOfCharacters[x, y].changeImage(tempSwapImage);

                                    Console.WriteLine(gridOfCharacters[x + 1, y].getBlockNum() + " Swapped With " + gridOfCharacters[x, y].getBlockNum());

                                }
                                else if (randomSwapper1 == 2 && x - 1 >= 0 && previousSwapNum != 1)
                                {
                                    previousSwapNum = 2;
                                    tempSwapImage = gridOfCharacters[x - 1, y].getImage();
                                    gridOfCharacters[x - 1, y].changeImage(whiteSquare);
                                    gridOfCharacters[x, y].changeImage(tempSwapImage);

                                    Console.WriteLine(gridOfCharacters[x - 1, y].getBlockNum() + " Swapped With " + gridOfCharacters[x, y].getBlockNum());

                                }
                                else if (randomSwapper1 == 3 && y - 1 >= 0 && previousSwapNum != 4)
                                {
                                    previousSwapNum = 3;
                                    tempSwapImage = gridOfCharacters[x, y - 1].getImage();
                                    gridOfCharacters[x, y - 1].changeImage(whiteSquare);
                                    gridOfCharacters[x, y].changeImage(tempSwapImage);

                                    Console.WriteLine(gridOfCharacters[x, y - 1].getBlockNum() + " Swapped With " + gridOfCharacters[x, y].getBlockNum());

                                }
                                else if (randomSwapper1 == 4 && y + 1 < gridOfCharacters.GetLength(1) && previousSwapNum != 3)
                                {
                                    previousSwapNum = 4;
                                    tempSwapImage = gridOfCharacters[x, y + 1].getImage();
                                    gridOfCharacters[x, y + 1].changeImage(whiteSquare);
                                    gridOfCharacters[x, y].changeImage(tempSwapImage);

                                    Console.WriteLine(gridOfCharacters[x, y + 1].getBlockNum() + " Swapped With " + gridOfCharacters[x, y].getBlockNum());

                                }


                            }

                        }

                    }

                }

            }

            isGameOver = false;
            Console.WriteLine("game over set to false");
            shouldTryCheckingAnswer = true;
            Console.WriteLine("should try checking answers set to true");

        }

        private void getImageParts(Bitmap ImageToSplit)
        {
            if (shouldTryCreatingNewImages)
            {
                imageInGrid1 = new Bitmap(Image.FromFile(importPath + originalFileToLoad + ".png"));

                tempPic1 = ImageToSplit;

                imageSizeX = ImageToSplit.Width / xTimesToDivideImage;
                imageSizeY = ImageToSplit.Height / yTimesToDivideImage;
            }
            else
            {
                imageSizeX = 200;
                imageSizeY = 200;

            }

            if (shouldTryCreatingNewImages == false)
            {
                convertBitmapToTexture2D();
            }

            int yIndex = 0;
            int xIndex = 0;
            imgCounter = 0;

            for (int y = 0; y < 600; y += imageSizeY)
            {

                for (int x = 0; x < 800; x += imageSizeX)
                {

                    //for (int y = 0; y < ImageToSplit.Height; y+= imageSizeY)
                    //{

                    //    for (int x = 0; x < ImageToSplit.Width; x+= imageSizeX)
                    //    {
                    //drawing.Rectangle = new drawing.Rectangle(drawing.Point, new Size(



                    ////**********For making new Images**************
                    //try
                    //{
                    if (shouldTryCreatingNewImages)
                    {
                        tempPic2 = tempPic1.Clone(new drawing.Rectangle(x, y, imageSizeX, imageSizeY), tempPic1.PixelFormat);
                        imageParts.Add(tempPic2);
                        tempPic2.Save(exportPath + @imagePrefixForFile + imgCounter.ToString() + ".png");
                    }
                    //}
                    //catch (OutOfMemoryException)
                    //{


                    //}


                    //Console.WriteLine("imgX" + tempPic2.Width);
                    //Console.WriteLine("imgY" + tempPic2.Height);

                    //recImageArray[imgCounter] = new xna.Rectangle(x, y, imageSizeX, imageSizeY);
                    try
                    {
                        listOfCharacters.Add(new Character(imagePartsAsTex2D[imgCounter], new xna.Rectangle(x, y, imageSizeX, imageSizeY)));
                    }
                    catch (IndexOutOfRangeException)
                    {

                    }
                    //gridOfCharacters[xIndex, yIndex] = listOfCharacters[imgCounter];



                    imgCounter += 1;
                    xIndex += 1;
                }
                yIndex += 1;
                xIndex = 0;

            }

            int c = 0;
            for (int y = 0; y < gridOfCharacters.GetLength(1); y++)
            {
                //nested for loop for 2nd dimension
                for (int x = 0; x < gridOfCharacters.GetLength(0); x++)
                {
                    gridOfCharacters[x, y] = listOfCharacters[c];


                    c++;
                }

            }

        }

        private void convertBitmapToTexture2D()
        {

            if (xTimesToDivideImage == 5 || yTimesToDivideImage == 5)
            {
                imagePrefixForFile = "imgCNSmall";
            }

            try
            {

                for (int i = 0; i < imagePartsAsTex2D.Length; i++)
                {
                    imagePartsAsTex2D[i] = Content.Load<Texture2D>("TilesForGrid/" + imagePrefixForFile + i);
                    imagePartsAsTex2DScrambled[i] = Content.Load<Texture2D>("TilesForGrid/" + imagePrefixForFile + i);
                }
                Console.WriteLine("1st try");

            }

            catch (ContentLoadException)
            {
                Console.WriteLine("1st catch");
                try
                {
                    for (int i = 0; i < imagePartsAsTex2D.Length; i++)
                    {
                        imagePartsAsTex2D[i] = Content.Load<Texture2D>("TilesForGrid/" + imagePrefixForFile + i);
                        imagePartsAsTex2DScrambled[i] = Content.Load<Texture2D>("TilesForGrid/" + imagePrefixForFile + i);
                    }
                    Console.WriteLine("went to try");

                }
                catch (ContentLoadException)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        imagePartsAsTex2D[i] = Content.Load<Texture2D>("TilesForGrid/" + imagePrefixForFile + i);
                        imagePartsAsTex2DScrambled[i] = Content.Load<Texture2D>("TilesForGrid/" + imagePrefixForFile + i);
                    }
                    Console.WriteLine("went to catch");
                }

            }

            imagePartsAsTex2D[0] = whiteSquare;
            imagePartsAsTex2DScrambled[0] = whiteSquare;


        }

        private void gamePlay(GameTime gameTime)
        {
            startOfGameCode();
            if (isAnimationActive == false)
            {

                traverseGridForUserInput();
                shiftTiles();
                checkAnswer();

            }

            if (delayTimer > 0)
            {
                delayTimer--;
            }
            if (delayTimer == 0)
            {
                scrambleGrid();
                delayTimer = -1;

            }
            userControls();
            if (isAnimationActive == true)
            {
                shiftAnimation(false, false, false, 50, 50);
            }

            gameClock++;
        }

        private void userControls()
        {
            //if (kb.IsKeyDown(Keys.T) && oldkb.IsKeyUp(Keys.T))
            //{
            //    setGridToDefault();

            //}

            if (kb.IsKeyDown(Keys.P) && oldkb.IsKeyUp(Keys.P))
            {
                scrambleGrid();

            }

            if (kb.IsKeyDown(Keys.Escape) && oldkb.IsKeyUp(Keys.Escape))
            {
                state = gameState.titleScreen;

            }

        }

        private void setSelectionRecPos()
        {
            for (int y = 0; y < gridOfCharacters.GetLength(1); y++)
            {
                //nested for loop for 2nd dimension
                for (int x = 0; x < gridOfCharacters.GetLength(0); x++)
                {
                    if (gridOfCharacters[x, y].getIsSelected() == true)
                    {
                        selectionIndicator.setCenter(gridOfCharacters[x, y].GetRekt);
                        //selectionIndicator.setY(gridOfCharacters[x, y].GetRekt.Center.Y);
                    }

                }

            }
            hasMoved = false;
            hasSelected = false;

        }

        private void traverseGridForUserInput()
        {
            for (int y = 0; y < gridOfCharacters.GetLength(1); y++)
            {
                //nested for loop for 2nd dimension
                for (int x = 0; x < gridOfCharacters.GetLength(0); x++)
                {

                    if (gridOfCharacters[x, y].getIsSelected() == true && isSelectionLocked == false && delayTimer == -1)
                    {


                        if ((kb.IsKeyDown(Keys.W) && oldkb.IsKeyUp(Keys.W)) || (kb.IsKeyDown(Keys.Up) && oldkb.IsKeyUp(Keys.Up)))
                        {

                            if (y - 1 != -1)
                            {
                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x, y - 1].setIsSelected(true);
                            }


                        }

                        else if ((kb.IsKeyDown(Keys.S) && oldkb.IsKeyUp(Keys.S)) || (kb.IsKeyDown(Keys.Down) && oldkb.IsKeyUp(Keys.Down)))
                        {
                            if (y + 1 != gridOfCharacters.GetLength(1) && hasMoved == false)
                            {
                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x, y + 1].setIsSelected(true);

                                hasMoved = true;
                            }

                        }



                        else if ((kb.IsKeyDown(Keys.D) && oldkb.IsKeyUp(Keys.D)) || (kb.IsKeyDown(Keys.Right) && oldkb.IsKeyUp(Keys.Right)))
                        {

                            if (x + 1 != gridOfCharacters.GetLength(0) && hasMoved == false)
                            {
                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x + 1, y].setIsSelected(true);
                                hasMoved = true;
                            }



                        }

                        else if ((kb.IsKeyDown(Keys.A) && oldkb.IsKeyUp(Keys.A)) || (kb.IsKeyDown(Keys.Left) && oldkb.IsKeyUp(Keys.Left)))
                        {
                            if (x - 1 != -1)
                            {
                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x - 1, y].setIsSelected(true);
                            }

                        }

                    }

                    if ((kb.IsKeyDown(Keys.Space) && oldkb.IsKeyUp(Keys.Space)) && hasSelected == false && gridOfCharacters[x, y].getImage() != whiteSquare && delayTimer == -1)
                    {
                        if (isSelectionLocked == false)
                        {
                            isSelectionLocked = true;

                        }
                        else
                        {
                            isSelectionLocked = false;
                        }

                        hasSelected = true;

                    }
                }
            }

            setSelectionRecPos();
            changeSelectionIndicator();


        }

        private void changeSelectionIndicator()
        {
            if (isSelectionLocked == false)
            {

                if (isSelectorIncreasing == true && selectorPulsarCounter <= 35)
                {
                    selectionIndicator.setWidth(selectionIndicator.GetRekt.Width + 1);
                    selectionIndicator.setHeight(selectionIndicator.GetRekt.Height + 1);
                    selectorPulsarCounter += 1;
                }
                else
                {
                    isSelectorIncreasing = false;
                }

                if (isSelectorIncreasing == false && selectorPulsarCounter >= 1)
                {
                    selectionIndicator.setWidth(selectionIndicator.GetRekt.Width - 1);
                    selectionIndicator.setHeight(selectionIndicator.GetRekt.Height - 1);

                    selectorPulsarCounter -= 1;

                }
                else
                {

                    isSelectorIncreasing = true;
                }

            }
            else if (isSelectionLocked == true)
            {
                selectionIndicator.setWidth(screenW / xTimesToDivideImage);
                selectionIndicator.setHeight(screenH / yTimesToDivideImage);
                isSelectorIncreasing = true;
                selectorPulsarCounter = 1;

            }
        }

        private void shiftTiles()
        {
            for (int y = 0; y < gridOfCharacters.GetLength(1); y++)
            {
                //nested for loop for 2nd dimension
                for (int x = 0; x < gridOfCharacters.GetLength(0); x++)
                {

                    if (gridOfCharacters[x, y].getIsSelected() == true && isSelectionLocked == true && gridOfCharacters[x, y].getImage() != whiteSquare)
                    {


                        if ((kb.IsKeyDown(Keys.W) && oldkb.IsKeyUp(Keys.W)) || (kb.IsKeyDown(Keys.Up) && oldkb.IsKeyUp(Keys.Up)))
                        {

                            if (y - 1 != -1 && gridOfCharacters[x, y - 1].getImage() == whiteSquare)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                tempAnimationImage2 = gridOfCharacters[x, y - 1].getImage();
                                shiftAnimation(true, true, false, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY);
                                gridOfCharacters[x, y - 1].changeImage(gridOfCharacters[x, y].getImage());
                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x, y - 1].setIsSelected(true);
                            }
                            else if (y - 2 >= 0 && gridOfCharacters[x, y - 2].getImage() == whiteSquare)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                tempAnimationImage2 = gridOfCharacters[x, y - 1].getImage();
                                shiftAnimation(true, true, false, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY, gridOfCharacters[x, y - 1].GetRektX, gridOfCharacters[x, y - 1].GetRektY);
                                gridOfCharacters[x, y - 2].changeImage(gridOfCharacters[x, y - 1].getImage());
                                gridOfCharacters[x, y - 1].changeImage(gridOfCharacters[x, y].getImage());

                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x, y - 1].setIsSelected(true);
                            }
                            else if (y - 3 >= 0 && gridOfCharacters[x, y - 3].getImage() == whiteSquare)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                tempAnimationImage2 = gridOfCharacters[x, y - 1].getImage();
                                tempAnimationImage3 = gridOfCharacters[x, y - 2].getImage();
                                shiftAnimation(true, true, false, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY, gridOfCharacters[x, y - 1].GetRektX, gridOfCharacters[x, y - 1].GetRektY,
                                    gridOfCharacters[x, y - 2].GetRektX, gridOfCharacters[x, y - 2].GetRektY);

                                gridOfCharacters[x, y - 3].changeImage(gridOfCharacters[x, y - 2].getImage());
                                gridOfCharacters[x, y - 2].changeImage(gridOfCharacters[x, y - 1].getImage());
                                gridOfCharacters[x, y - 1].changeImage(gridOfCharacters[x, y].getImage());

                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x, y - 1].setIsSelected(true);
                            }


                        }

                        else if ((kb.IsKeyDown(Keys.S) && oldkb.IsKeyUp(Keys.S)) || (kb.IsKeyDown(Keys.Down) && oldkb.IsKeyUp(Keys.Down)))
                        {
                            if (y + 1 < gridOfCharacters.GetLength(1) && hasMoved == false && gridOfCharacters[x, y + 1].getImage() == whiteSquare)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                shiftAnimation(true, false, false, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY);
                                gridOfCharacters[x, y + 1].changeImage(gridOfCharacters[x, y].getImage());
                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x, y + 1].setIsSelected(true);
                                hasMoved = true;

                            }
                            else if (y + 2 < gridOfCharacters.GetLength(1) && gridOfCharacters[x, y + 2].getImage() == whiteSquare)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                tempAnimationImage2 = gridOfCharacters[x, y + 1].getImage();
                                shiftAnimation(true, false, false, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY, gridOfCharacters[x, y + 1].GetRektX, gridOfCharacters[x, y + 1].GetRektY);
                                gridOfCharacters[x, y + 2].changeImage(gridOfCharacters[x, y + 1].getImage());
                                gridOfCharacters[x, y + 1].changeImage(gridOfCharacters[x, y].getImage());

                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x, y + 1].setIsSelected(true);
                            }
                            else if (y + 3 < gridOfCharacters.GetLength(1) && gridOfCharacters[x, y + 3].getImage() == whiteSquare)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                tempAnimationImage2 = gridOfCharacters[x, y + 1].getImage();
                                tempAnimationImage3 = gridOfCharacters[x, y + 2].getImage();
                                shiftAnimation(true, false, false, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY, gridOfCharacters[x, y + 1].GetRektX, gridOfCharacters[x, y + 1].GetRektY,
                                    gridOfCharacters[x, y + 2].GetRektX, gridOfCharacters[x, y + 2].GetRektY);

                                gridOfCharacters[x, y + 3].changeImage(gridOfCharacters[x, y + 2].getImage());
                                gridOfCharacters[x, y + 2].changeImage(gridOfCharacters[x, y + 1].getImage());
                                gridOfCharacters[x, y + 1].changeImage(gridOfCharacters[x, y].getImage());

                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x, y + 1].setIsSelected(true);
                            }

                        }



                        else if ((kb.IsKeyDown(Keys.D) && oldkb.IsKeyUp(Keys.D)) || (kb.IsKeyDown(Keys.Right) && oldkb.IsKeyUp(Keys.Right)))
                        {

                            if (x + 1 < gridOfCharacters.GetLength(0) && hasMoved == false && gridOfCharacters[x + 1, y].getImage() == whiteSquare)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                shiftAnimation(false, false, false, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY);
                                gridOfCharacters[x + 1, y].changeImage(gridOfCharacters[x, y].getImage());
                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x + 1, y].setIsSelected(true);
                                hasMoved = true;

                            }
                            else if (x + 2 < gridOfCharacters.GetLength(0) && hasMoved == false && gridOfCharacters[x + 2, y].getImage() == whiteSquare)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                tempAnimationImage2 = gridOfCharacters[x + 1, y].getImage();
                                shiftAnimation(false, false, false, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY, gridOfCharacters[x + 1, y].GetRektX, gridOfCharacters[x + 1, y].GetRektY);
                                gridOfCharacters[x + 2, y].changeImage(gridOfCharacters[x + 1, y].getImage());
                                gridOfCharacters[x + 1, y].changeImage(gridOfCharacters[x, y].getImage());

                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x + 1, y].setIsSelected(true);
                                hasMoved = true;

                            }
                            else if (x + 3 < gridOfCharacters.GetLength(0) && hasMoved == false && gridOfCharacters[x + 3, y].getImage() == whiteSquare)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                tempAnimationImage2 = gridOfCharacters[x + 1, y].getImage();
                                tempAnimationImage3 = gridOfCharacters[x + 2, y].getImage();
                                shiftAnimation(false, false, false, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY, gridOfCharacters[x + 2, y].GetRektX, gridOfCharacters[x + 2, y].GetRektY,
                                    gridOfCharacters[x + 3, y].GetRektX, gridOfCharacters[x + 3, y].GetRektY);

                                gridOfCharacters[x + 3, y].changeImage(gridOfCharacters[x + 2, y].getImage());
                                gridOfCharacters[x + 2, y].changeImage(gridOfCharacters[x + 1, y].getImage());
                                gridOfCharacters[x + 1, y].changeImage(gridOfCharacters[x, y].getImage());

                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x + 1, y].setIsSelected(true);
                                hasMoved = true;

                            }


                        }

                        else if ((kb.IsKeyDown(Keys.A) && oldkb.IsKeyUp(Keys.A)) || (kb.IsKeyDown(Keys.Left) && oldkb.IsKeyUp(Keys.Left)))
                        {
                            if (x - 1 >= 0 && gridOfCharacters[x - 1, y].getImage() == whiteSquare && hasMoved == false)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                shiftAnimation(false, false, true, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY);
                                gridOfCharacters[x - 1, y].changeImage(gridOfCharacters[x, y].getImage());
                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x - 1, y].setIsSelected(true);
                                hasMoved = true;

                            }
                            else if (x - 2 >= 0 && gridOfCharacters[x - 2, y].getImage() == whiteSquare && hasMoved == false)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                tempAnimationImage2 = gridOfCharacters[x - 1, y].getImage();
                                shiftAnimation(false, false, true, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY, gridOfCharacters[x - 2, y].GetRektX, gridOfCharacters[x - 2, y].GetRektY);
                                gridOfCharacters[x - 2, y].changeImage(gridOfCharacters[x - 1, y].getImage());
                                gridOfCharacters[x - 1, y].changeImage(gridOfCharacters[x, y].getImage());

                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x - 1, y].setIsSelected(true);
                                hasMoved = true;

                            }
                            else if (x - 3 >= 0 && gridOfCharacters[x - 3, y].getImage() == whiteSquare && hasMoved == false)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                tempAnimationImage2 = gridOfCharacters[x - 1, y].getImage();
                                tempAnimationImage3 = gridOfCharacters[x - 2, y].getImage();
                                shiftAnimation(false, false, true, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY, gridOfCharacters[x - 2, y].GetRektX, gridOfCharacters[x - 2, y].GetRektY,
                                    gridOfCharacters[x - 3, y].GetRektX, gridOfCharacters[x - 3, y].GetRektY);

                                gridOfCharacters[x - 3, y].changeImage(gridOfCharacters[x - 2, y].getImage());
                                gridOfCharacters[x - 2, y].changeImage(gridOfCharacters[x - 1, y].getImage());
                                gridOfCharacters[x - 1, y].changeImage(gridOfCharacters[x, y].getImage());

                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x - 1, y].setIsSelected(true);
                                hasMoved = true;

                            }




                            /*else if (x + 3 < gridOfCharacters.GetLength(0) && hasMoved == false && gridOfCharacters[x + 3, y].getImage() == whiteSquare)
                            {
                                shouldGetAnimationStartTime = true;
                                tempAnimationImage1 = gridOfCharacters[x, y].getImage();
                                tempAnimationImage2 = gridOfCharacters[x + 1, y].getImage();
                                tempAnimationImage3 = gridOfCharacters[x + 2, y].getImage();
                                shiftAnimation(false, false, false, gridOfCharacters[x, y].GetRektX, gridOfCharacters[x, y].GetRektY, gridOfCharacters[x + 2, y].GetRektX, gridOfCharacters[x + 2, y].GetRektY,
                                    gridOfCharacters[x + 3, y].GetRektX, gridOfCharacters[x + 3, y].GetRektY);

                                gridOfCharacters[x + 3, y].changeImage(gridOfCharacters[x + 2, y].getImage());
                                gridOfCharacters[x + 2, y].changeImage(gridOfCharacters[x + 1, y].getImage());
                                gridOfCharacters[x + 1, y].changeImage(gridOfCharacters[x, y].getImage());

                                gridOfCharacters[x, y].changeImage(whiteSquare);

                                gridOfCharacters[x, y].setIsSelected(false);
                                gridOfCharacters[x + 1, y].setIsSelected(true);
                                hasMoved = true;

                            }
                             * 
                             * */

                        }


                    }


                }

            }


        }

        private void shiftAnimation(bool isVertical, bool isGoingUp, bool isGoingLeft, int startX, int startY, int startX2 = -500, int startY2 = - 500,
            int startX3 = -500, int startY3 = - 500)
        {
            if (shouldGetAnimationStartTime == true)
            {
                animationStartTime = gameClock;
                shouldGetAnimationStartTime = false;
                isAnimationVertical = isVertical;
                isAnimationGoingUp = isGoingUp;
                isAnimationGoingLeft = isGoingLeft;

                animationCharacter1.setX(startX);
                animationCharacter1.setY(startY);
                animationCharacter1.changeImage(tempAnimationImage1);
                animationCharacter2.setX(startX2);
                animationCharacter2.setY(startY2);
                animationCharacter2.changeImage(tempAnimationImage2);
                animationCharacter3.setX(startX3);
                animationCharacter3.setY(startY3);
                animationCharacter3.changeImage(tempAnimationImage3);
            }

            if (animationEndTime - animationStartTime < 47)
            {
                isAnimationActive = true;

                if (isAnimationVertical == true)
                {
                    if (isAnimationGoingUp)
                    {
                        selectionIndicator.setY(selectionIndicator.GetRektY - 4);
                        animationCharacter2.setY(selectionIndicator.GetRektY - selectionIndicator.GetRekt.Height);
                        animationCharacter3.setY(selectionIndicator.GetRektY - selectionIndicator.GetRekt.Height * 2);
                    }
                    else
                    {
                        selectionIndicator.setY(selectionIndicator.GetRektY + 4);
                        animationCharacter2.setY(selectionIndicator.GetRektY + selectionIndicator.GetRekt.Height);
                        animationCharacter3.setY(selectionIndicator.GetRektY + selectionIndicator.GetRekt.Height * 2);
                    }
                    animationCharacter1.setY(selectionIndicator.GetRektY);
                    //animationCharacter2.setY(selectionIndicator.GetRektY + selectionIndicator.GetRekt.Height);
                    //animationCharacter3.setY(selectionIndicator.GetRektY + selectionIndicator.GetRekt.Height * 2);
                }
                else if (isAnimationVertical == false)
                {
                    if (isAnimationGoingLeft)
                    {
                        selectionIndicator.setX(selectionIndicator.GetRektX - 4);
                        animationCharacter2.setX(selectionIndicator.GetRektX - selectionIndicator.GetRekt.Width);
                        animationCharacter3.setX(selectionIndicator.GetRektX - selectionIndicator.GetRekt.Width * 2);
                    }
                    else
                    {
                        selectionIndicator.setX(selectionIndicator.GetRektX + 4);
                        animationCharacter2.setX(selectionIndicator.GetRektX + selectionIndicator.GetRekt.Width);
                        animationCharacter3.setX(selectionIndicator.GetRektX + selectionIndicator.GetRekt.Width * 2);

                    }
                    animationCharacter1.setX(selectionIndicator.GetRektX);

                }
            }
            else
            {
                isAnimationActive = false;
                isSelectionLocked = false;
                animationStartTime = 0;
                animationEndTime = 0;
                isAnimationGoingLeft = false;
                isAnimationGoingUp = false;
                isAnimationVertical = false;
                tempAnimationImage1 = whiteSquare;
                tempAnimationImage2 = whiteSquare;
                tempAnimationImage3 = whiteSquare;

            }


            animationEndTime = gameClock;
        }

        private void resetGridForStartOfGame()
        {
            gridPosCounter = 0;
            for (int y = 0; y < gridOfCharacters.GetLength(1); y++)
            {
                //nested for loop for 2nd dimension
                for (int x = 0; x < gridOfCharacters.GetLength(0); x++)
                {
                    gridOfCharacters[x, y].setIsSelected(false);
                    gridOfCharacters[x, y].setBlockNum(gridPosCounter);
                    gridOfCharacters[x, y].setStartingNum(gridPosCounter);

                    gridPosCounter += 1;
                }

            }

            gridOfCharacters[0, 0].changeImage(whiteSquare);
            gridOfCharacters[1, 1].setIsSelected(true);
            //selectionIndicator.setX(gridOfCharacters[0, 0].GetRektX);
            //selectionIndicator.setY(gridOfCharacters[0, 0].GetRektY);


        }

        private void assignColor()
        {
            if (colorChoiceNum == 1)
            {
                colorChoice = xna.Color.LightBlue;
            }
            else if (colorChoiceNum == 2)
            {
                colorChoice = xna.Color.Blue;
            }
            else if (colorChoiceNum == 3)
            {
                colorChoice = xna.Color.Red;
            }
            else if (colorChoiceNum == 4)
            {
                colorChoice = xna.Color.Pink;
            }

        }

        //for when the player is on the title screen
        private void titleScreen()
        {

            startOfGameCode();

            if ((playButton.GetRekt).Contains(new xna.Point(mouseState.X, mouseState.Y)) || kb.IsKeyDown(Keys.Up))
            {
                startButtonSelected = true;
                optionsButtonSelected = false;


            }

            else if ((optionsButton.GetRekt).Contains(new xna.Point(mouseState.X, mouseState.Y)) || kb.IsKeyDown(Keys.Down))
            {
                optionsButtonSelected = true;
                startButtonSelected = false;

            }
            else
            {
                optionsButtonSelected = false;
                startButtonSelected = false;

            }
            //else
            //{
            //    optionsButtonSelected = false;
            //    startButtonSelected = false;
            //}



            if (optionsButtonSelected)
            {

                if ((mouseState.LeftButton == ButtonState.Pressed && oldmouseState.LeftButton == ButtonState.Released) || kb.IsKeyDown(Keys.Enter))
                {
                    //startButtonSelected = false;
                    //MediaPlayer.Pause();
                    getImageParts(imageInGrid1);
                    state = gameState.options;
                }

            }
            else if (startButtonSelected)
            {

                if (mouseState.LeftButton == ButtonState.Pressed || kb.IsKeyDown(Keys.Enter))
                {

                    hasDoneStartOfGameCode = false;
                    state = gameState.gamePlay;
                }

            }




        }
        //for when the player is on the title screen
        private void optionsScreen()
        {

            if ((goBackButton.GetRekt).Contains(new xna.Point(mouseState.X, mouseState.Y)))
            {
                goBackButtonSelected = true;

                if (mouseState.LeftButton == ButtonState.Pressed && oldmouseState.LeftButton == ButtonState.Released)
                {

                    state = gameState.titleScreen;

                }

            }
            else if ((puzzleChoiceButton.GetRekt).Contains(new xna.Point(mouseState.X, mouseState.Y)) || kb.IsKeyDown(Keys.Up))
            {
                goBackButtonSelected = false;
                puzzleChoiceButtonSelected = true;
                difficultyButtonSelected = false;

                if ((mouseState.LeftButton == ButtonState.Pressed && oldmouseState.LeftButton == ButtonState.Released) || (kb.IsKeyDown(Keys.Right) && oldkb.IsKeyUp(Keys.Right)))
                {
                    puzzleChoiceNum += 1;


                    if (puzzleChoiceNum > totalNumOfPuzzles)
                    {
                        puzzleChoiceNum = 1;
                    }

                    if (puzzleChoiceNum == 1)
                    {
                        puzzleChoicePreview.changeImage(previewImages[0]);
                        imagePrefixForFile = "blobsBorder";
                    }
                    else if (puzzleChoiceNum == 2)
                    {
                        puzzleChoicePreview.changeImage(previewImages[1]);
                        imagePrefixForFile = "imgSQI";
                    }
                    else if (puzzleChoiceNum == 3)
                    {
                        puzzleChoicePreview.changeImage(previewImages[2]);
                        imagePrefixForFile = "imgCoding";
                    }

                    convertBitmapToTexture2D();
                    setGridToDefault();


                }

                else if ((mouseState.RightButton == ButtonState.Pressed && oldmouseState.RightButton == ButtonState.Released) || (kb.IsKeyDown(Keys.Left) && oldkb.IsKeyUp(Keys.Left)))
                {
                    puzzleChoiceNum -= 1;

                    if (puzzleChoiceNum < 1)
                    {
                        puzzleChoiceNum = totalNumOfPuzzles;
                    }

                    if (puzzleChoiceNum == 1)
                    {
                        puzzleChoicePreview.changeImage(previewImages[0]);
                        imagePrefixForFile = "blobsBorder";
                    }
                    else if (puzzleChoiceNum == 2)
                    {
                        puzzleChoicePreview.changeImage(previewImages[1]);
                        imagePrefixForFile = "imgSQI";
                    }
                    else if (puzzleChoiceNum == 3)
                    {
                        puzzleChoicePreview.changeImage(previewImages[2]);
                        imagePrefixForFile = "imgCoding";
                    }

                    convertBitmapToTexture2D();
                    setGridToDefault();

                }





            }
            else if ((difficultyButton.GetRekt).Contains(new xna.Point(mouseState.X, mouseState.Y)) || kb.IsKeyDown(Keys.Down))
            {
                goBackButtonSelected = false;
                puzzleChoiceButtonSelected = false;
                difficultyButtonSelected = true;

                if ((mouseState.LeftButton == ButtonState.Pressed && oldmouseState.LeftButton == ButtonState.Released) || (kb.IsKeyDown(Keys.Right) && oldkb.IsKeyUp(Keys.Right)))
                {
                    if (difficultyNum <= 9)
                    {
                        difficultyNum++;
                    }

                }

                else if ((mouseState.RightButton == ButtonState.Pressed && oldmouseState.RightButton == ButtonState.Released) || (kb.IsKeyDown(Keys.Left) && oldkb.IsKeyUp(Keys.Left)))
                {
                    if (difficultyNum > 1)
                    {
                        difficultyNum--;
                    }

                }

            }

            else
            {
                goBackButtonSelected = false;
                puzzleChoiceButtonSelected = false;
                difficultyButtonSelected = false;
            }


            if (kb.IsKeyDown(Keys.Escape))
            {
                state = gameState.titleScreen;

            }


            assignColor();


        }
        //for when the player is on the end screen
        private void endScreen()
        {


        }

        private void Lose()
        {
            if ((goToTitleScreenButton.GetRekt).Contains(new xna.Point(mouseState.X, mouseState.Y)))
            {
                goToTitleScreenButtonSelected = true;

                if (mouseState.LeftButton == ButtonState.Pressed && oldmouseState.LeftButton == ButtonState.Released)
                {
                    state = gameState.titleScreen;
                }

            }
            else
            {
                goToTitleScreenButtonSelected = false;
            }

        }
        private void Win()
        {

        }

        private void drawTitleScreen()
        {
            GraphicsDevice.Clear(xna.Color.White);



            spriteBatch.DrawString(titleAndEndFont, "Picture Tiles", new Vector2(250, 110), xna.Color.Black);

            if (startButtonSelected == true)
            {
                playButton.DrawCharacter(spriteBatch, xna.Color.Red);
            }
            else
                playButton.DrawCharacter(spriteBatch, xna.Color.White);

            if (optionsButtonSelected == true)
            {
                optionsButton.DrawCharacter(spriteBatch, xna.Color.Blue);
            }
            else
                optionsButton.DrawCharacter(spriteBatch, xna.Color.White);

        }

        private void drawGamePlay()
        {

            for (int y = 0; y < gridOfCharacters.GetLength(1); y++)
            {

                for (int x = 0; x < gridOfCharacters.GetLength(0); x++)
                {
                    //gridOfCharacters[x, y].DrawCharacter(spriteBatch);

                    if (gridOfCharacters[x, y].getImage() != tempAnimationImage1 && gridOfCharacters[x, y].getImage() != tempAnimationImage2 &&
                        gridOfCharacters[x, y].getImage() != tempAnimationImage3)
                    {
                        gridOfCharacters[x, y].DrawCharacter(spriteBatch);

                    }

                }

            }

            if (isAnimationActive == true)
            {
                animationCharacter1.DrawCharacter(spriteBatch);
                animationCharacter2.DrawCharacter(spriteBatch);
                animationCharacter3.DrawCharacter(spriteBatch);
            }

            if (isGameOver == true)
            {
                spriteBatch.DrawString(titleAndEndFont, "  You Win!\nPress P to\nRescramble\nOr Escape\nFor Title", new Vector2(screenW / 2 - 130, screenH / 2 - 80), xna.Color.Red);



            }

            if (delayTimer > 0)
            {
                spriteBatch.DrawString(titleAndEndFont, "Scrambling in " + (int)(delayTimer / 60), new Vector2(screenW / 2 - 130, screenH / 2 - 80), xna.Color.Red);


            }

            if (delayTimer == -1)
            {
                selectionIndicator.DrawCharacter(spriteBatch);
            }
        }
        private void drawLose()
        {

            if (goToTitleScreenButtonSelected == true)
            {
                goToTitleScreenButton.DrawCharacter(spriteBatch, xna.Color.Red);
            }
            else
                goToTitleScreenButton.DrawCharacter(spriteBatch, xna.Color.White);

            spriteBatch.DrawString(titleAndEndFont, "You ran out of lives!", new Vector2(190, 110), xna.Color.Black);

            spriteBatch.DrawString(buttonFont, "Go to Title Screen", new Vector2((goToTitleScreenButton.GetRektX + (goToTitleScreenButton.GetRektXLength / 4) - 50),
                (goToTitleScreenButton.GetRektY - (goToTitleScreenButton.GetRektYLength / 4))), xna.Color.Black);

        }
        private void drawEndScreen()
        {

        }
        private void drawOptionsScreen()
        {

            if (goBackButtonSelected == true)
            {
                goBackButton.DrawCharacter(spriteBatch, xna.Color.Red);
            }
            else
                goBackButton.DrawCharacter(spriteBatch, xna.Color.White);

            if (puzzleChoiceButtonSelected == true)
            {
                puzzleChoiceButton.DrawCharacter(spriteBatch, xna.Color.Red);
            }
            else
                puzzleChoiceButton.DrawCharacter(spriteBatch, xna.Color.White);

            if (difficultyButtonSelected == true)
            {
                difficultyButton.DrawCharacter(spriteBatch, xna.Color.Red);
            }
            else
                difficultyButton.DrawCharacter(spriteBatch, xna.Color.White);


            spriteBatch.DrawString(customfont, "    In Game:\nMove Selection With\nArrow keys or WASD \nkeys\nLock/unlock selection with\nSpacebar\nOnce locked, use the\n" +
            "arrow key or WASD key for the\ndirection\nYou can only slide into the\nblank square\nTry to complete the picture",
                new Vector2(50, screenH - 400), xna.Color.Black);


            spriteBatch.DrawString(buttonFont, "Go Back", new Vector2((goBackButton.GetRektX + ((goBackButton.GetRektXLength) / 4)),
               (goBackButton.GetRektY + (goBackButton.GetRektYLength / 4))), xna.Color.Black);

            spriteBatch.DrawString(buttonFont, "Puzzle", new Vector2((puzzleChoiceButton.GetRektX + ((puzzleChoiceButton.GetRektXLength) / 4)),
               (puzzleChoiceButton.GetRektY + (puzzleChoiceButton.GetRektYLength / 4))), xna.Color.Black);

            spriteBatch.DrawString(buttonFont, "Difficulty", new Vector2((difficultyButton.GetRektX + 10),
               (difficultyButton.GetRektY + (difficultyButton.GetRektYLength / 4))), xna.Color.Black);

            puzzleChoicePreview.DrawCharacter(spriteBatch);

            spriteBatch.DrawString(buttonFont, difficultyNum.ToString(), new Vector2((difficultyButton.GetRektX + ((difficultyButton.GetRektXLength) / 4)) + 170,
                (difficultyButton.GetRektY - (difficultyButton.GetRektYLength / 4)) + 30), xna.Color.Black);



            //display to the right
            //colorChoiceDisplay.DrawCharacter(spriteBatch, colorChoice);
            //spriteBatch.DrawString(buttonFont, stateOfTimerMode, new Vector2((enableTimedModeButton.GetRektX + ((enableTimedModeButton.GetRektXLength) / 4)) + 170,
            //    (enableTimedModeButton.GetRektY - (enableTimedModeButton.GetRektYLength / 4))), xna.Color.Black);
            //spriteBatch.DrawString(buttonFont, areLivesEnabled.ToString(), new Vector2((livesButton.GetRektX + ((livesButton.GetRektXLength) / 4)) + 170,
            //    (livesButton.GetRektY - (livesButton.GetRektYLength / 4))), xna.Color.Black);

            spriteBatch.DrawString(customfont, "   Left Click to Add\nRight Click to Subtract", new Vector2(screenW - 270, screenH - 80), xna.Color.Black);

        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(xna.Color.White);

            spriteBatch.Begin();

            switch (state)
            {

                case gameState.titleScreen:

                    drawTitleScreen();

                    break;
                case gameState.gamePlay:

                    drawGamePlay();
                    break;
                case gameState.Lose:

                    drawLose();
                    break;

                case gameState.endScreen:

                    drawEndScreen();

                    break;

                case gameState.options:

                    drawOptionsScreen();

                    break;

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
