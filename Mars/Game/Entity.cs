﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    public class Entity
    {
        private string _ID;                 // The unique identifier of this entity.

        private Vector2 _position;
        private Vector2 _tile_position;

        public int _waypoint;
        private bool _selected;

        private float _width;
        private float _height;
        private float _radius;

        private bool _dynamic;              //If the entity is dynamic, it will have physics applied to it.
        
        private string _type;               //What type of entity is it.

        private Vector2 _velocity;          //The velocity, directly impacts position every frame.
        private Vector2 _previous_velocity; //The velocity in the previous frame.
        private Vector2 _acceleration;      //The acceleration. The rate of change applied to velocity every frame. Can be decceleration too.
        private Vector2 _direction;         //The direction vector for the entity.

        private float _resistance;          //The modifier to the force applied in the opposite direction to which the entity is moving.

        private bool _in_motion;            //Whether or not the entity is moving.

        private float _max_velocity;        //The maximum speed which the entity can move.

        public Entity()
        {
            _width = 0;
            _height = 0;
            _radius = 0;

            _dynamic = false;
        }

        public Entity(Vector2 position, float width, float height, float radius, bool dynamic, string type)
        {
            _dynamic = dynamic;

            _type = type;

            _velocity = new Vector2(0, 0);
            _acceleration = new Vector2(0, 0);
            _direction = new Vector2(0, 0);

            _resistance = 5;

            _in_motion = false;

            _max_velocity = 10;

            Position = position;
            Width = width;
            Height = height;
            Radius = radius;

        }

        public virtual void Collide(Entity entity)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if (_dynamic)
            {
                //ApplyPhysics(gameTime);
            }
        }

        public void ApplyPhysics(GameTime gameTime)
        {
            // velocity += acceleration * deltaT;
            // position += velocity * deltaT;

            //If in motion, add acceleration to velocity and update position.
            if (_in_motion)
            {
                //If the velocity is below it's max velocity value.
                if(_velocity.LengthSquared() < _max_velocity)
                {
                    _velocity += _acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else //If the velocity is not below it's max velocity value then deccelerate it.
                {
                    _direction = Vector2.Normalize(_velocity);

                    _acceleration = new Vector2((_direction.X * _resistance), (_direction.Y * _resistance));

                    _acceleration = _acceleration * -1;

                    _velocity += _acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                //If the velocity has not reached 0, deccelerate in the direciton it's moving. Else, stop it.
                if (_velocity != Vector2.Zero)
                {
                    _direction = Vector2.Normalize(_velocity);

                    _acceleration = new Vector2((_direction.X * _resistance), (_direction.Y * _resistance));

                    _acceleration = _acceleration * -1;
                }
                else
                {
                    _in_motion = false;
                }

                //Update the position.
                Position += _velocity;

                //Cleanup the small velocity and decceleration values.
                if (System.Math.Abs(_velocity.X) < 0.1f)
                {
                    _velocity = new Vector2(0.0f, Velocity.Y);
                    _acceleration.X = 0;
                }
                if (System.Math.Abs(_velocity.Y) < 0.1f)
                {
                    _velocity = new Vector2(Velocity.X, 0.0f);
                    _acceleration.Y = 0;
                }
            }

            _previous_velocity = _velocity;
        }

        public void ApplyForce(Vector2 force)
        {
            _acceleration += force;
            _in_motion = true;
        }

        /// <summary>
        /// Gets the radius of the entity. If the height and width are different the radius isn't determined and just the (width / 2) is returned.
        /// </summary>
        /// <returns>The radius of the entity</returns>
        public float GetRadius()
        {
            float w = _width / 2;
            float h = _height / 2;

            if (w == h)
            {
                _radius = w;
            }
            else
            {
                _radius = w;
                Console.WriteLine("The width and height of this Entity are not the same, so it doesn't have a set radius. Value set to (width/2)");
            }

            return _radius;
        }

        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 TilePosition
        {
            get { return _tile_position; }
            set { _tile_position = value; }
        }

        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public bool Dynamic
        {
            get { return _dynamic; }
            set { _dynamic = value; }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public float MaxVelocity
        {
            get { return _max_velocity; }
            set { _max_velocity = value; }
        }

        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public float Resistance
        {
            get { return _resistance; }
            set { _resistance = value; }
        }

        public Vector2 Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }
    }
}
