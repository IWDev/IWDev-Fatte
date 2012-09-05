// Learn more about F# at http://fsharp.net

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input


// ****************************************
// THE GAME CLASS
// ****************************************
type XnaGame() as this =
    inherit Game()
    
    do this.Content.RootDirectory <- "XnaGameContent"
    let graphicsDeviceManager = new GraphicsDeviceManager(this)
    
    // Lots of deliberately mutable stuff
    let mutable spriteBatch : SpriteBatch = null
    let mutable texCat      : Texture2D = null
    let mutable texFurball  : Texture2D = null
    let mutable texAlien    : Texture2D = null
    let mutable rectCat     : Rectangle = new Rectangle()
    let mutable rectFurball : Rectangle = new Rectangle()
    let mutable rectAlien   : Rectangle = new Rectangle()
    let mutable alienTarget : Point = new Point()
    let randNum             : Random = new Random()

    // Initialize function as member override
    override game.Initialize() =
        graphicsDeviceManager.GraphicsProfile <- GraphicsProfile.HiDef
        graphicsDeviceManager.PreferredBackBufferWidth <- 800
        graphicsDeviceManager.PreferredBackBufferHeight <- 600
        graphicsDeviceManager.ApplyChanges() 
        spriteBatch <- new SpriteBatch(game.GraphicsDevice)
        base.Initialize()

    // LoadContent function as member override
    override game.LoadContent() =
        texCat <- this.Content.Load<Texture2D>("cat")
        texFurball <- this.Content.Load<Texture2D>("furball")
        texAlien <- this.Content.Load<Texture2D>("alien")

        rectCat <- new Rectangle(50, 100, texCat.Width, texCat.Height)
        rectFurball <- new Rectangle(1000, 0, texFurball.Width, texFurball.Height)
        rectAlien <- new Rectangle(550, 100, texAlien.Width, texAlien.Height)

        alienTarget <- new Point(650, randNum.Next(600 - rectAlien.Height))
        
    // Update function as member override
    override game.Update gameTime = 
        if GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed then this.Exit()

        if GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.2f || Keyboard.GetState().IsKeyDown(Keys.Up) then
            rectCat.Offset(0, -4)
        if GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.2f || Keyboard.GetState().IsKeyDown(Keys.Down) then
            rectCat.Offset(0, 4)

        if GamePad.GetState(PlayerIndex.One).Buttons.A = ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space) then
            rectFurball.Location <- new Point(rectCat.Location.X + 100, rectCat.Location.Y + 50)

        if rectAlien.Top > alienTarget.Y + 5 then 
            rectAlien.Offset(0, -4)
        else if rectAlien.Top < alienTarget.Y - 5 then 
            rectAlien.Offset(0, 4)
        else alienTarget <- new Point(550, randNum.Next(600 - rectAlien.Height))

        rectFurball.Offset(10, 0)

        if rectFurball.Intersects(rectAlien) then 
            rectFurball.X <- 1000
            rectAlien.Y <- -rectAlien.Height

    // Draw function as member override
    override game.Draw gameTime = 
        game.GraphicsDevice.Clear(Color.CornflowerBlue)
        spriteBatch.Begin()
        
        spriteBatch.Draw(texCat, rectCat, Color.White)
        spriteBatch.Draw(texFurball, rectFurball, Color.White)
        spriteBatch.Draw(texAlien, rectAlien, Color.White)

        spriteBatch.End()


// ****************************************
// START THE GAME!!
// ****************************************
let game = new XnaGame()
game.Run()