
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class AIControlledEntity : Entity
    {
        private const int AttackDistance = 151070;
        private const int DetectDistance = 282140;

        public enum AIState
        {
            Intercept,      //Move in attack position
            Wander,         //Randomly patrol until we meet an opponent
            Shooting,       //Fire at will!
            Search          //Find out how gain line of sight to our opponent
        }

        private AIState _state;
        private readonly List<Entity> _entities;
        private readonly TileMap _tileMap;
        private Stack<AStarPathfinder.Node> _waypoints;
        private AStarPathfinder.Node _currentWaypoint;
        private readonly AStarPathfinder _pathfinder;

        private Entity _target;
        private Entity _leader;
        private readonly Random rand = new Random();
        private long _waypointTimeout = 0;

        public AIControlledEntity(UnitType type, List<Entity> entities, TileMap world, Entity leader = null) : base(type)
        {
            Team = 'A'; //Team AI
            _tileMap = world;
            _entities = entities;
            _state = AIState.Wander;
            _waypoints = new Stack<AStarPathfinder.Node>();
            _pathfinder = new AStarPathfinder();
            _leader = leader;
        }

        public override void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse)
        {
            Point targetTile;
            double distance = double.MaxValue;

            //Calculate distance to our target
            if (_target != null)
            {
                distance = (_target.Position - Position).LengthSquared();
                if (_target.IsDestroyed)
                {
                    _target = null;
                    _state = AIState.Wander;
                    _waypoints.Clear();
                }
            }

            //Clear all waypoints if we use more than 2 seconds to reach the next waypoint
            if (_waypointTimeout < delta.TotalGameTime.TotalMilliseconds)
            {
                _waypointTimeout = (int)delta.TotalGameTime.TotalMilliseconds + 1500;
                _waypoints.Clear();
                _currentWaypoint = null;
            }

            //Get next waypoint?
            if (_currentWaypoint == null && _waypoints.Count > 0)
            {
                _currentWaypoint = _waypoints.Pop();
                _waypointTimeout = (int)delta.TotalGameTime.TotalMilliseconds + 1500;
            }

            //Move towards next waypoint
            if (_currentWaypoint != null)
            {
                //Reached our waypoint?
                targetTile = _tileMap.GetSquareAtPixel(Position);
                if (targetTile.X == _currentWaypoint.X && targetTile.Y == _currentWaypoint.Y)
                {
                    _currentWaypoint = null;
                }
                else
                {
                    Point ourTile = _tileMap.GetSquareAtPixel(Position);
                    float targetRotation = (float) (Math.PI/2 - Math.Atan2(_currentWaypoint.X - ourTile.X, _currentWaypoint.Y - ourTile.Y));

                    //Need to adjust our rotation?
                    if (Math.Abs(targetRotation - Rotation) > 0.1)
                    {
                        if (targetRotation > Rotation)      Rotation += TurnRate * 0.25f;   //Make AI turn 1/4th of normal
                        else if (targetRotation < Rotation) Rotation -= TurnRate * 0.25f;   //(I think it looks more natural)
                    }
                    else
                    {
                        Rotation = targetRotation;
                    }

                    //Move towards waypoint!
                    VelX = (float)Math.Cos(Rotation) * Speed;
                    VelY = (float)Math.Sin(Rotation) * Speed;
                }
            }

            //Determine AI behaviour
            switch (_state)
            {
                case AIState.Wander:

                    //Look for enemies
                    foreach (var entity in _entities.Where(entity => entity.Team != Team))
                    {
                        distance = (entity.Position - Position).LengthSquared();
                        if (distance > DetectDistance) continue;
                        _target = entity;
                        _state = AIState.Intercept;
                        break;
                    }

                    if (_currentWaypoint == null)
                    {
                        //Follow our leader
                        if (_leader != null)
                        {
                            targetTile = _tileMap.GetSquareAtPixel(_leader.Position);
                            _currentWaypoint = new AStarPathfinder.Node(targetTile.X, targetTile.Y);

                            //Has our leader died?
                            if (_leader.IsDestroyed) _leader = null;
                        }

                        //Randomly wander around
                        else
                        {
                            Point ourTile = _tileMap.GetSquareAtPixel(Position+new Vector2(Width, Height));
                            targetTile = _tileMap.GetSquareAtPixel(new Vector2(rand.Next(_tileMap.RealWidth), rand.Next(_tileMap.RealHeight)));
                            if (_tileMap.IsWalkable(targetTile.X, targetTile.Y)) _waypoints = _pathfinder.FindPath(targetTile.X, targetTile.Y, ourTile.X, ourTile.Y, _tileMap);
                        }
                    }

                    break;

                case AIState.Search:
                    if (_target == null)
                    {
                        _state = AIState.Wander;
                        break;
                    }

                    //Not found a path yet?
                    if (_waypoints.Count == 0)
                    {
                        //Determine new path
                        Point ourTile = _tileMap.GetSquareAtPixel(Position);
                        targetTile = _tileMap.GetSquareAtPixel(_target.Position);
                        _waypoints = _pathfinder.FindPath(ourTile.X, ourTile.Y, targetTile.X, targetTile.Y, _tileMap);

                        //No path found to target?
                        if (_waypoints.Count == 0)
                        {
                            _state = AIState.Wander;
                        }
                        else
                        {
                            //Remove first waypoint (our position)
                            _currentWaypoint = _waypoints.Pop();
                        }
                    }
                        
                    //Did we find our target?
                    else if (HasLineOfSight(_target) && distance < AttackDistance)
                    {
                        _state = AIState.Shooting;
                        _waypoints.Clear();
                    }

                    break;

                case AIState.Intercept:
                    if (_target == null)
                    {
                        _state = AIState.Wander;
                        break;
                    }

                    //Lost line of sight? Then find a path
                    if (!HasLineOfSight(_target))
                    {
                        _state = AIState.Search;
                    }

                    //Within attack range?
                    else if (distance < AttackDistance)
                    {
                        _state = AIState.Shooting;
                    }

                    //Move towards target
                    else
                    {
                        _waypoints.Clear();
                        targetTile = _tileMap.GetSquareAtPixel(_target.Position);
                        _currentWaypoint = new AStarPathfinder.Node(targetTile.X, targetTile.Y);
                    }

                    break;

                case AIState.Shooting:
                    if (_target == null)
                    {
                        _state = AIState.Wander;
                        break;
                    }

                    distance = (_target.Position - Position).LengthSquared();

                    //Lost sight of our target?
                    if (!HasLineOfSight(_target) || distance > AttackDistance)
                    {
                        _state = AIState.Intercept;
                    }
                    else
                    {
                        Rotation = (float)(Math.PI / 2 - Math.Atan2(_target.X - X, _target.Y - Y));
                        FireProjectile();
                    }

                    break;
            }


            base.Update(delta, keyboard, mouse);
        }

        // Swap the values of A and B
        private void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        private bool HasLineOfSight(GameObject target)
        {
            //Our cartesian position
            int x0 = _tileMap.GetSquareByPixelX((int)X);
            int y0 = _tileMap.GetSquareByPixelY((int)Y);

            //Their cartesian position
            int x1 = _tileMap.GetSquareByPixelX((int)target.X);
            int y1 = _tileMap.GetSquareByPixelY((int)target.Y);

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            //Bresenhams Line Algorithm for ray tracing
            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);
            int error = 0;
            int ystep;
            int y = y0;
            if (y0 < y1) ystep = 1; else ystep = -1;
            for (int x = x0; x <= x1; x++)
            {
                bool walkable = steep ? _tileMap.IsWalkable(y, x) :_tileMap.IsWalkable(x, y);

                if (!walkable) return false;

                error += deltay;
                if (2*error < deltax) continue;
                y += ystep;
                error -= deltax;
            }

            return true;
        }

    }
}
