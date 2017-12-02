using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using GameLibrary;



namespace LunarLander2
{
    public class Lander : DrawableGameComponent
    {
        private class LanderParts //Class to better organize the parts of the lander for crashing
        {
            public List<Vector2> Part { get; set; }
            public Vector2 Position { get; set; }
            public float XSpeed;
            public float YSpeed;
       
            public LanderParts(List<Vector2> vertices, Vector2 position)
            {
                Part = vertices;
                Position = position;
                XSpeed = 0.0f;
                YSpeed = 0.0f;
            }

            public LanderParts(LanderParts lander)
            {
                Part = new List<Vector2>(lander.Part);
                Position = new Vector2(lander.Position.X, lander.Position.Y);
                XSpeed = lander.XSpeed;
                YSpeed = lander.YSpeed;
            }
        }

        public enum landerState
        {
            playing,
            crashed,
            landed
        }

        #region variables       
        private PrimitiveBatch pb;
        private SoundEffect rocketBoosterEffect;
        private SoundEffectInstance rocketSound;
        private bool isThrusting;
        private KeyboardState oldKeyboardState;
        private LanderParts landerCan;
        private LanderParts manCan;
        private LanderParts thruster;
        private LanderParts thrust1;
        private LanderParts thrust2;
        private List<Vector2> thrust1Unmodified;
        private List<Vector2> thrust2Unmodified;
        private LanderParts legRight;
        private LanderParts legLeft;
        private LanderParts fullLander;
        private List<Vector2> fullLanderUnmodified;
        private Color landerColor;
        private Color thrustColor;
        private Vector2 thrustLocation;
        private float thrustPower;
        private float landerScale = 1.25f;
        Vector2 landerCenter;
        float angleRad;
        int angleDeg;
        float gravity  = 1.6f;
        float gravityPerMS;
        #endregion

        public List<Vector2> Points
        {
            get
            {
                List<Vector2> points = new List<Vector2>();
                foreach (Vector2 v2 in fullLander.Part)
                {
                    points.Add(v2 + fullLander.Position);
                }
                return points;            }
        }

        public landerState State
        {
            get;
            private set;
        }

        public float Fuel
        {
            get;
            private set;
        }

        public float XSpeed
        {
            get
            {
                
                return Math.Abs(fullLander.XSpeed);
            }
            //private set { Speed = value; }
        }

        public float YSpeed
        {
            get
            {
                return Math.Abs(fullLander.YSpeed);
            }
        }

        public int Angle
        {
            get { return angleDeg; }
        }

        public void Start()
        {
            State = landerState.playing;
            fullLander.XSpeed = 0;
            fullLander.YSpeed = 0;
            fullLander.Position = new Vector2(15.0f, 5.0f);
        }
        
        public void Crash() //runs when the game tells us we crashed
        {
            Random rand = new Random();
            State = landerState.crashed;//our state is crashed now

            manCan.Position = fullLander.Position;//set the part to the location of the lander that just crashed
            CrashHelper(rand, manCan); //use the crash helper to give it a random direction and power

            landerCan.Position = fullLander.Position;
            CrashHelper(rand, landerCan);

            legLeft.Position = fullLander.Position;
            CrashHelper(rand, legLeft);

            legRight.Position = fullLander.Position;
            CrashHelper(rand, legRight);

            thruster.Position = fullLander.Position;
            CrashHelper(rand, thruster);

            fullLander.Position = new Vector2(0, 0);
        }

        private void CrashHelper(Random rand, LanderParts part) //helps generate directions for the parts so that we get a scatter spray of parts
        {
            part.XSpeed = (float)(rand.Next(0,40) - 20) / 10.0f;
            part.YSpeed = (float)rand.NextDouble();
        }

        public void winGame()
        {
            State = landerState.landed;
            fullLander.XSpeed = 0;
            fullLander.YSpeed = 0;
        }

        public Lander(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            pb = new PrimitiveBatch(Game.GraphicsDevice);
            isThrusting = false;
            gravityPerMS = gravity / 1000.0f;
            thrustPower = 0 - (gravityPerMS * 2.0f);
            VectorFont.Initialize(Game);
            fullLander = new LanderParts(new List<Vector2>(), new Vector2(15.0f, 5.0f));
            Fuel = 100;
            angleRad = MathHelper.ToRadians(0);

            rocketBoosterEffect = Game.Content.Load<SoundEffect>("RocketThrust");
            
            #region initialize parts
            landerCan = new LanderParts(LanderTextToVector.GetPoints(string.Format("{0}\\{1}", Game.Content.RootDirectory, "LanderCan.txt")), fullLander.Position);
            
            manCan = new LanderParts(LanderTextToVector.GetPoints(string.Format("{0}\\{1}", Game.Content.RootDirectory, "ManCap.txt")), fullLander.Position);

            thrust1 = new LanderParts(LanderTextToVector.GetPoints(string.Format("{0}\\{1}", Game.Content.RootDirectory, "Thrust.txt")), fullLander.Position);
            thrust2 = new LanderParts(LanderTextToVector.GetXMirrorPoints(string.Format("{0}\\{1}", Game.Content.RootDirectory, "Thrust.txt")), fullLander.Position);
                        
            thruster = new LanderParts(LanderTextToVector.GetPoints(string.Format("{0}\\{1}", Game.Content.RootDirectory, "Thruster.txt")), fullLander.Position);

            legRight = new LanderParts(LanderTextToVector.GetPoints(string.Format("{0}\\{1}", Game.Content.RootDirectory, "LegR.txt")), fullLander.Position);
            legLeft = new LanderParts(LanderTextToVector.GetXMirrorPoints(string.Format("{0}\\{1}", Game.Content.RootDirectory, "LegR.txt")), fullLander.Position);
            #endregion

            #region construct lander
            LinkedList<Vector2> tempLander = new LinkedList<Vector2>();
            Vector2 partPosition = new Vector2(0f, 0f);

            foreach (Vector2 v2 in manCan.Part)
            {
                tempLander.AddLast(v2 + partPosition);
            }
            partPosition.Y += 4;
            foreach (Vector2 v2 in landerCan.Part)
            {
                tempLander.AddLast(v2 + partPosition);
            }
            partPosition.Y += 3;
            partPosition.X -= 5;
            foreach (Vector2 v2 in legLeft.Part)
            {
                tempLander.AddLast(v2 + partPosition);
            }
            partPosition.X += 11;
            foreach (Vector2 v2 in legRight.Part)
            {
                tempLander.AddLast(v2 + partPosition);
            }
            partPosition.X -= 5;
            partPosition.Y += 3;
            foreach (Vector2 v2 in thruster.Part)
            {
                tempLander.AddLast(v2 + partPosition);
            }
            
            partPosition.Y += 1;
            
            thrustLocation = new Vector2(partPosition.X * landerScale, partPosition.Y * landerScale);
            LinkedListNode<Vector2> node = tempLander.First;

            for (int i = 0; i < tempLander.Count; ++i)
            {
                node.Value *= landerScale;
                node = node.Next;
                if (i < thrust1.Part.Count)
                {
                    thrust1.Part[i] *= landerScale;
                    thrust2.Part[i] *= landerScale;
                }
            }

            float minimumLanderXPosition = 0;
            float maximumLanderXPosition = 0;
            float minimumLanderYPosition = 0;
            float maximumLanderYPosition = 0;

            foreach(Vector2 v2 in tempLander)
            {
                if (minimumLanderXPosition > v2.X)
                {
                    minimumLanderXPosition = v2.X;
                }
                if (maximumLanderXPosition < v2.X)
                {
                    maximumLanderXPosition = v2.X;
                }
                if (minimumLanderYPosition > v2.Y)
                {
                    minimumLanderYPosition = v2.Y;
                }
                if (maximumLanderYPosition < v2.Y)
                {
                    maximumLanderYPosition = v2.Y;
                }
            }
            Vector2 cornerModifier = new Vector2(0 - minimumLanderXPosition, 0 - minimumLanderYPosition);
            thrustLocation += cornerModifier;
            for (int i = 0; i < thrust1.Part.Count; ++i)
            {
                thrust1.Part[i] += thrustLocation;
                thrust2.Part[i] += thrustLocation;
            }
            landerCenter = new Vector2((maximumLanderXPosition - minimumLanderXPosition) / 2, (maximumLanderYPosition - minimumLanderYPosition) / 2);
            foreach(Vector2 v2 in tempLander)
            {
                fullLander.Part.Add(v2 + cornerModifier);
            }
            #endregion

            Start();
            oldKeyboardState = new KeyboardState();
            landerColor = Color.Silver;
            thrustColor = Color.OrangeRed;
            fullLanderUnmodified = new List<Vector2>(fullLander.Part);
            thrust1Unmodified = new List<Vector2>(thrust1.Part);
            thrust2Unmodified = new List<Vector2>(thrust2.Part);
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            
            KeyboardState newKeyboardState = Keyboard.GetState();
            float thisGravity = gravityPerMS * gameTime.ElapsedGameTime.Milliseconds;
            if (State == landerState.playing)
            {
                fullLander.YSpeed += thisGravity;
            }else if (State == landerState.crashed)
            {
                manCan.YSpeed += thisGravity;
                landerCan.YSpeed += thisGravity;
                legLeft.YSpeed += thisGravity;
                legRight.YSpeed += thisGravity;
                thruster.YSpeed += thisGravity;
            }

            if (State == landerState.playing)
            {
                if (newKeyboardState.IsKeyDown(Keys.Left) && newKeyboardState != oldKeyboardState)
                {
                    angleDeg -= 15;
                    /*if (angleDeg < -90)
                    {
                        angleDeg = -90;
                    }*/
                    rotationMath();
                }
                if (newKeyboardState.IsKeyDown(Keys.Right) && newKeyboardState != oldKeyboardState)
                {
                    angleDeg += 15;
                    /*if (angleDeg > 90)
                    {
                        angleDeg = 90;
                    }*/
                    rotationMath();
                }

                if (newKeyboardState.IsKeyDown(Keys.Space))
                {
                    if (Fuel > 0)
                    {
                        //position += new Vector2(0, -1);
                        ThrustMath(gameTime);
                        Fuel -= 0.1f;
                        if (Fuel < 0)
                        {
                            Fuel = 0.000f;
                        }
                        isThrusting = true;
                    }
                    else
                    {
                        isThrusting = false;
                    }
                }else
                {
                    isThrusting = false;
                }//end thrust
                
                oldKeyboardState = newKeyboardState;
            }
            else
            {
                
                isThrusting = false;
            }

            

            if (State == landerState.playing || State == landerState.landed)
            {
                fullLander.Position = new Vector2(fullLander.Position.X + (fullLander.XSpeed), fullLander.Position.Y + (fullLander.YSpeed));
            }else
            {
                manCan.Position = new Vector2(manCan.Position.X + (manCan.XSpeed ), manCan.Position.Y + (manCan.YSpeed));
                landerCan.Position = new Vector2(landerCan.Position.X + (landerCan.XSpeed ), landerCan.Position.Y + (landerCan.YSpeed));
                legLeft.Position = new Vector2(legLeft.Position.X + (legLeft.XSpeed), legLeft.Position.Y + (legLeft.YSpeed));
                legRight.Position = new Vector2(legRight.Position.X + (legRight.XSpeed ), legRight.Position.Y + (legRight.YSpeed));
                thruster.Position = new Vector2(thruster.Position.X + (thruster.XSpeed ), thruster.Position.Y + (thruster.YSpeed));
            }

            handleSound();
            base.Update(gameTime);
        }

        private void handleSound()
        {
            if (isThrusting)
            {
                if (rocketSound == null)
                {
                    rocketSound = rocketBoosterEffect.CreateInstance();
                    rocketSound.IsLooped = true;
                    rocketSound.Play();
                }
            }
            else
            {
                if (rocketSound != null)
                {
                    rocketSound.Stop();
                    rocketSound.Dispose();
                    rocketSound = null;
                }
            }
        }

        private void rotationMath()
        {
            angleRad = MathHelper.ToRadians(angleDeg);
            double cosTheta = Math.Cos(angleRad);
            double sinTheta = Math.Sin(angleRad);
            for (int i = 0; i < fullLanderUnmodified.Count; ++i)
            {
                Vector2 LR = new Vector2();
                LR.X = (float)(cosTheta * (fullLanderUnmodified[i].X - landerCenter.X) - sinTheta * (fullLanderUnmodified[i].Y - landerCenter.Y) + landerCenter.X);
                LR.Y = (float)(sinTheta * (fullLanderUnmodified[i].X - landerCenter.X) + cosTheta * (fullLanderUnmodified[i].Y - landerCenter.Y) + landerCenter.Y);
                fullLander.Part[i] = LR;
            }
            for (int i = 0; i < thrust1.Part.Count; ++i)
            {
                Vector2 LR = new Vector2();
                LR.X = (float)(cosTheta * (thrust1Unmodified[i].X - landerCenter.X) - sinTheta * (thrust1Unmodified[i].Y - landerCenter.Y) + landerCenter.X);
                LR.Y = (float)(sinTheta * (thrust1Unmodified[i].X - landerCenter.X) + cosTheta * (thrust1Unmodified[i].Y - landerCenter.Y) + landerCenter.Y);
                thrust1.Part[i] = LR;
                LR = new Vector2();
                LR.X = (float)(cosTheta * (thrust2Unmodified[i].X - landerCenter.X) - sinTheta * (thrust2Unmodified[i].Y - landerCenter.Y) + landerCenter.X);
                LR.Y = (float)(sinTheta * (thrust2Unmodified[i].X - landerCenter.X) + cosTheta * (thrust2Unmodified[i].Y - landerCenter.Y) + landerCenter.Y);
                thrust2.Part[i] = LR;
            }
        }

        private void ThrustMath(GameTime gameTime)
        {
            float thisThrust = thrustPower * gameTime.ElapsedGameTime.Milliseconds;
            fullLander.XSpeed += (float)(thisThrust * Math.Sin(0-angleRad));
            fullLander.YSpeed += (float)(thisThrust * Math.Cos(0-angleRad));
            thrust1.XSpeed = thrust2.XSpeed = fullLander.XSpeed;
            thrust1.YSpeed = thrust2.YSpeed = fullLander.YSpeed;
        }

        public override void Draw(GameTime gameTime)
        {
            if (State == landerState.playing || State == landerState.landed)
            {
                pb.Begin(PrimitiveType.LineList);

                foreach(Vector2 v2 in fullLander.Part)
                {
                    pb.AddVertex(v2 + fullLander.Position, landerColor);
                }

                if (isThrusting)
                {
                    
                    if (gameTime.ElapsedGameTime.Ticks % 2 == 0)
                    {
                        thrust1.Position = fullLander.Position;
                        foreach (Vector2 v2 in thrust1.Part)
                        {
                            pb.AddVertex(v2 + thrust1.Position, thrustColor);
                        }
                    }else
                    {
                        thrust2.Position = fullLander.Position;
                        foreach (Vector2 v2 in thrust2.Part)
                        {
                            pb.AddVertex(v2 + thrust2.Position, thrustColor);
                        }
                    }
                }

                pb.End();
            }
            else
            {
                pb.Begin(PrimitiveType.LineList);
                foreach (Vector2 v2 in manCan.Part)
                {
                    pb.AddVertex(v2 + manCan.Position, landerColor);
                }
                
                foreach (Vector2 v2 in landerCan.Part)
                {
                    pb.AddVertex(v2 + landerCan.Position, landerColor);
                }
                
                foreach (Vector2 v2 in legLeft.Part)
                {
                    pb.AddVertex(v2 + legLeft.Position, landerColor);
                }
                
                foreach (Vector2 v2 in legRight.Part)
                {
                    pb.AddVertex(v2 + legRight.Position, landerColor);
                }
                
                foreach (Vector2 v2 in thruster.Part)
                {
                    pb.AddVertex(v2 + thruster.Position, landerColor);
                }
                pb.End();
            }
            
            

            base.Draw(gameTime);
        }
    }
}
