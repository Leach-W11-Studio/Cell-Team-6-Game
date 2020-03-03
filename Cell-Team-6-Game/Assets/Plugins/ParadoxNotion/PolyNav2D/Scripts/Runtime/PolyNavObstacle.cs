using UnityEngine;
using UnityEngine.U2D;
using System.Collections;
using System.Collections.Generic;

namespace PolyNav
{

    [DisallowMultipleComponent]
    [AddComponentMenu("Navigation/PolyNavObstacle")]
    ///Place on a game object to act as an obstacle
    public class PolyNavObstacle : MonoBehaviour
    {

        public enum ShapeType
        {
            Polygon,
            Box,
            Composite,
            Edge
        }

        ///Raised when the state of the obstacle is changed (enabled/disabled).
        public static event System.Action<PolyNavObstacle, bool> OnObstacleStateChange;

        [Tooltip("The Shape used. Changing this will also change the Collider2D component type.")]
        public ShapeType shapeType = ShapeType.Polygon;
        [Tooltip("Added extra offset radius.")]
        public float extraOffset;
        [Tooltip("Inverts the polygon (done automatically if collider already exists due to a sprite).")]
        public bool invertPolygon = false;
        public float edgeWidth = 1;

        public SpriteShapeController spriteShapeController;
        private Spline spline {
            get { 
                    if (spriteShapeController) {
                        return spriteShapeController.spline;
                    }
                    else {
                        spriteShapeController = GetComponent<SpriteShapeController>();
                        return spriteShapeController.spline;
                    }
                }
        }

        private Collider2D _collider;
        private Collider2D myCollider {
            get { return _collider != null ? _collider : _collider = GetComponent<Collider2D>(); }
        }

        public Mesh shapeMesh;

        private void Start() {
            if(myCollider is EdgeCollider2D) {
                shapeMesh = myCollider.CreateMesh(true, true);
            }
        }

        ///The number of paths defining the obstacle
        public int GetPathCount() {
            if ( myCollider is BoxCollider2D ) { return 1; }
            if ( myCollider is PolygonCollider2D ) { return ( myCollider as PolygonCollider2D ).pathCount; }
            if ( myCollider is CompositeCollider2D ) { return ( myCollider as CompositeCollider2D ).pathCount; }
            if ( myCollider is EdgeCollider2D ) { return 1; }
            return 0;
        }

        ///Returns the points defining a path
        public Vector2[] GetPathPoints(int index) {
            Vector2[] points = null;
            if ( myCollider is BoxCollider2D ) {
                var box = (BoxCollider2D)myCollider;
                var tl = box.offset + ( new Vector2(-box.size.x, box.size.y) / 2 );
                var tr = box.offset + ( new Vector2(box.size.x, box.size.y) / 2 );
                var br = box.offset + ( new Vector2(box.size.x, -box.size.y) / 2 );
                var bl = box.offset + ( new Vector2(-box.size.x, -box.size.y) / 2 );
                points = new Vector2[] { tl, tr, br, bl };
            }

            if ( myCollider is PolygonCollider2D ) {
                var poly = (PolygonCollider2D)myCollider;
                points = poly.GetPath(index);
            }

            if ( myCollider is CompositeCollider2D ) {
                var comp = (CompositeCollider2D)myCollider;
                points = new Vector2[comp.GetPathPointCount(index)];
                comp.GetPath(index, points);
            }

            if (myCollider is EdgeCollider2D) {
                return GetEdgePath();
            }

            if ( invertPolygon && points != null ) { System.Array.Reverse(points); }
            return points;
        }

        void Reset() {

            if ( myCollider == null ) {
                gameObject.AddComponent<PolygonCollider2D>();
                invertPolygon = true;
            }

            if ( myCollider is PolygonCollider2D ) {
                shapeType = ShapeType.Polygon;
            }

            if ( myCollider is BoxCollider2D ) {
                shapeType = ShapeType.Box;
            }

            if ( myCollider is CompositeCollider2D ) {
                shapeType = ShapeType.Composite;
            }

            if (myCollider is EdgeCollider2D) {
                shapeType = ShapeType.Edge;
            }

            myCollider.isTrigger = true;
            if ( GetComponent<SpriteRenderer>() != null ) {
                invertPolygon = true;
            }
        }

        void OnEnable() {
            if ( OnObstacleStateChange != null ) {
                OnObstacleStateChange(this, true);
            }
        }

        void OnDisable() {
            if ( OnObstacleStateChange != null ) {
                OnObstacleStateChange(this, false);
            }
        }

        void Awake() {
            transform.hasChanged = false;

            if (myCollider is EdgeCollider2D) {
                spriteShapeController = GetComponent<SpriteShapeController>();
            }
        }

        List<Vector2> GetEdgeNormals () {
            List<Vector2> normals = new List<Vector2>();
            if (!(myCollider is EdgeCollider2D)) { return normals;}
            EdgeCollider2D edgeCollider = (EdgeCollider2D) myCollider;

            bool open = spline.isOpenEnded;

            int index = 0;
            foreach (var point in edgeCollider.points) {
                var normal = new Vector2();
                if ( index == 0 ) {
                    if (open) {
                        normal = Vector2.Perpendicular(edgeCollider.points[index + 1] - point).normalized;
                    }
                    else {
                        var perp1 = Vector2.Perpendicular(point - edgeCollider.points[edgeCollider.pointCount - 1]).normalized;
                        var perp2 = Vector2.Perpendicular(edgeCollider.points[index + 1] - point).normalized;
                        normal = Vector2.Lerp(perp1, perp2, 0.5f);
                    }
                }
                else if ( index == edgeCollider.pointCount-1 ) {
                    if (open) {
                        normal = Vector2.Perpendicular(point - edgeCollider.points[index - 1]).normalized;
                    }
                    else {
                        var perp1 = Vector2.Perpendicular(point - edgeCollider.points[index - 1]).normalized;
                        var perp2 = Vector2.Perpendicular(edgeCollider.points[0] - point).normalized;
                        normal = Vector2.Lerp(perp1, perp2, 0.5f);
                    }
                }
                else {
                    normal = Vector2.Perpendicular(edgeCollider.points[index + 1] - edgeCollider.points[index - 1]).normalized;
                }

                normals.Add(normal);
                index++;
                Debug.DrawRay(transform.TransformPoint(point), normal, Color.green);
            }

            return normals;
        }

        private Vector2[] GetEdgePath () {
            List<Vector2> normals = GetEdgeNormals();
            Vector2[] colliderPoints = ((EdgeCollider2D) myCollider).points;
            Vector2[] edgePoints = new Vector2[colliderPoints.Length];
            Vector2[] innerPoints = new Vector2[colliderPoints.Length];
            Vector2[] path;
            bool open = spline.isOpenEnded;

            int index = 0;
            foreach (Vector2 colliderPoint in colliderPoints) {
                edgePoints[index] = colliderPoint;
                innerPoints[index] = colliderPoint - (normals[index] * edgeWidth);
                index++;
            }

            System.Array.Reverse(innerPoints);

            path = new Vector2[edgePoints.Length * 2];
            edgePoints.CopyTo(path, 0);
            innerPoints.CopyTo(path, edgePoints.Length);

            System.Array.Reverse(path);

            return path;
        }

        // void OnDrawGizmos() {
        //     if (myCollider is EdgeCollider2D) {
        //         if (!spriteShapeController) {Awake();}
        //         Gizmos.color = Color.white;
        //         List<Vector2> normals = GetEdgeNormals();
        //         int index = 0;
        //         foreach (var point in ((EdgeCollider2D) myCollider).points) {
        //             Gizmos.DrawWireSphere(transform.TransformPoint(point), 0.2f);
        //             Vector2 otherPoint = (Vector2) transform.TransformPoint(point - normals[index]);
        //             Gizmos.DrawWireSphere(otherPoint, 0.2f);
        //             index++;
        //         }
        //     }
        // }
    }
}