using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
public class Level1MapController : MonoBehaviour
{
    public class KeyPoint
    {
        public KeyPoint(List<double> position, int sideIndex, int gridIndex, bool isCross, string belongTo, string pointName)
        {
            this.position = position;
            this.gridIndex = gridIndex;
            this.isCross = isCross;
            this.belongTo = belongTo;
            this.pointName = pointName;
            this.sideIndex = sideIndex;
        }
        public List<double> position;
        public int sideIndex;
        public int gridIndex;
        public bool isCross;
        public string belongTo;
        public string pointName;
        public string toString()
        {
            return pointName + " side index:" + this.sideIndex + " gridIndex:" + this.gridIndex + " belongto:" + this.belongTo;
        }
        public string belongToSide()
        {
            return this.belongTo + this.sideIndex;
        }
        public string printPosition()
        {
            return (this.position[0] + " " + this.position[1] + " " + this.position[2]);
        }
        public void setPosition(List<double> position)
        {
            this.position = position;
        }
    }
    public class Route
    {
        public Character character;
        private bool onGoing = false;
        private int surfaceCount = 1;
        private List<string> route;
        private int index;
        private string surfaceName;
        private string finalRoute;
        private Transform finalPosition;
        public Dictionary<string, KeyPoint> cntKeyPointMap;
        public Dictionary<string, Dictionary<string, KeyPoint>> keyPointMap;
        public Route()
        {
            this.route = new List<string>();
        }
        public Route(Dictionary<string, Dictionary<string, KeyPoint>> keyPointMap, Character character)
        {
            this.keyPointMap = keyPointMap;
            this.character = character;
        }
        public void reset(List<string> route, string srcRouteName, Transform tarPos, string tarRoute)
        {
            this.index = 0;
            this.setRoute(route, srcRouteName, tarRoute);
            this.setFinalPosition(tarPos);
            this.setFinalRoute(tarRoute);
            this.surfaceCount = 1;
        }
        public bool isGoing()
        {
            return this.onGoing;
        }
        public void setOnGoing(bool isGoing)
        {
            this.onGoing = isGoing;
        }
        //TODO 此处错误的采用了对象副本，应当为引用
        public void setkeyPointMap(Dictionary<string, Dictionary<string, KeyPoint>> keyPointMap)
        {
            this.keyPointMap = keyPointMap;
            //getNextPointMap("C");
        }
        public Transform nextPoint()
        {
            if (index >= route.Count) return this.finalPosition;
            //Debug.Log("Next Point is " + route[index]);
            return GameObject.Find(route[index]).transform;
            //return GameOjbec  route[index]].position;
        }
        public void goNextPoint()
        {
            this.index++;

            if (index < this.route.Count)
            {
                if (getSurfaceName(route[index - 1]) != getSurfaceName(route[index]))
                {
                    //character.transform.SetParent(GameObject.Find("Plane" + getSurfaceName(route[index])).transform, true);
                    character.cntSurface = getSurfaceName(route[index]);
                    //character.transform.Rotate(new Vector3(0, 0, 1), -90);
                    character.transform.position = GameObject.Find(route[index]).transform.position;
                }
                Debug.Log("Last Reach is " + cntKeyPointMap[route[index - 1]].position[0] + " " + cntKeyPointMap[route[index - 1]].position[1] + " " + cntKeyPointMap[route[index - 1]].position[2] + " ");
                if (getSurfaceName(route[index]) != getSurfaceName(route[index - 1]))
                    getNextPointMap(getSurfaceName(route[index]));
            }
            else
            {
                if (index == this.route.Count)
                    Debug.Log("Last Reach is " + cntKeyPointMap[route[index - 1]].position[0] + " " + cntKeyPointMap[route[index - 1]].position[1] + " " + cntKeyPointMap[route[index - 1]].position[2] + " ");
                else if (index == this.route.Count + 1)
                    Debug.Log("final Point is " + finalPosition.position[0] + " " + finalPosition.position[1] + " " + finalPosition.position[2] + " ");
                else
                    Debug.Log("END");
                //Debug.Log("larger than route count "+this.index+" "+this.route.Count);
            }
        }
        private void getNextPointMap(string surfaceName)
        {
            this.cntKeyPointMap = keyPointMap[surfaceName];
        }
        public void setSurfaceCount(int surfaceCount)
        {
            this.surfaceCount = surfaceCount;
        }
        public bool hasRoute()
        {
            return route.Count != 0 && index <= route.Count;
        }
        private string getNumberInPointStr(string pointName)
        {
            //Debug.Log("point name "+pointName.Substring(6, pointName.Length - 6));
            return pointName.Substring(6, pointName.Length - 6);
        }
        public void setRoute(List<string> route, string srcRoute, string tarRoute)
        {
            this.route = route;
            string[] startPoints = srcRoute.Split(new char[] { '-' });
            string[] endPoints = tarRoute.Split(new char[] { '-' });
            if (route.Count > 1)
            {
                if (startPoints.Length == 3)
                {
                    if (
                    (startPoints[1].Equals(getNumberInPointStr(route[0])) && startPoints[2].Equals(getNumberInPointStr(route[1])))
                    || (startPoints[1].Equals(getNumberInPointStr(route[1])) && startPoints[2].Equals(getNumberInPointStr(route[0])))
                    )
                        this.index++;
                }
                int count = route.Count;
                if (endPoints.Length == 3)
                {
                    if (
                        (endPoints[1].Equals(getNumberInPointStr(route[count - 1])) && endPoints[2].Equals(getNumberInPointStr(route[count - 2])))
                    || (endPoints[1].Equals(getNumberInPointStr(route[count - 2])) && endPoints[2].Equals(getNumberInPointStr(route[count - 1])))
                    )
                        this.route.RemoveAt(count - 1);
                }

            }
        }
        public void setSurfaceName(string surfaceName)
        {
            this.surfaceName = surfaceName;
        }
        public void setFinalPosition(Transform finalPosition)
        {
            this.finalPosition = finalPosition;
        }
        public Transform getFinalPosition()
        {
            return this.finalPosition;
        }
        public void setFinalRoute(string finalRoute)
        {
            this.finalRoute = finalRoute;
        }
        public string getFinalRoute()
        {
            return this.finalRoute;
        }
    }
    public Route globalRoute;
    public Character character;

    //TODO verify whether these two number can be one
    public static int mapSize = 8;
    public static int gridCount = 6;
    //public Dictionary<string, List<string>> adjacentSurface = new Dictionary<string,List<string>>();
    public Dictionary<string, int[,]> surfaceMap = new Dictionary<string, int[,]>();
    /**key means surfaces names, value means index of edge (ex.AC-3)*/
    public Dictionary<string, int> surfaceEdgeMap = new Dictionary<string, int>();
    /**first means index of edge(0-11 in cube),second means twe sides in the edge(ex.5-C3&A0)*/
    public string[,] edgeSide;
    /**map of side-exit position string-List:A3(third side of surface A)-3(index of list is 0, 3 means the exit position)*/
    public Dictionary<string, List<int>> sideExitMap = new Dictionary<string, List<int>>();
    public Dictionary<string, Dictionary<string, KeyPoint>> keyPointMap = new Dictionary<string, Dictionary<string, KeyPoint>>();

    private void showRoute(List<string> route)
    {
        string str = "";
        for (int i = 0; i < route.Count; i++)
        {
            str += (route[i] + " " + keyPointMap[getSurfaceName(route[i])][route[i]].printPosition() + "\n");
        }
        Debug.Log("route is " + str);
    }
    public bool isSameRoute(string src, string tar)
    {
        return false;
    }
    public bool isSameSurface(string src, string tar)
    {
        Debug.Log(src + " " + tar);
        return src.Equals(tar);
    }
    public int isAdjacent(string srcSurfaceName, string tarSurfaceName)
    {
        Debug.Log(srcSurfaceName + tarSurfaceName);
        if (surfaceEdgeMap.ContainsKey(srcSurfaceName + tarSurfaceName))
            return surfaceEdgeMap[srcSurfaceName + tarSurfaceName];
        if (surfaceEdgeMap.ContainsKey(tarSurfaceName + srcSurfaceName))
            return surfaceEdgeMap[tarSurfaceName + srcSurfaceName];

        return -1;
    }
    /** get src surface join point list from two adjacent surfaces */
    //public List<KeyPoint> getSideMatchExitList(List<int> side1, List<int> side2, string surface1, string surface2)
    public List<KeyPoint> getSideMatchExitList(int edgeIndex, string surface1, string surface2)
    {
        List<KeyPoint> list = new List<KeyPoint>();
        Dictionary<string, KeyPoint> srcPointList = keyPointMap[surface1];
        Dictionary<string, KeyPoint> tarPointList = keyPointMap[surface2];
        string srcSide = edgeSide[edgeIndex, 0].Contains(surface1) ? edgeSide[edgeIndex, 0] : edgeSide[edgeIndex, 1];
        string tarSide = edgeSide[edgeIndex, 0].Contains(surface2) ? edgeSide[edgeIndex, 0] : edgeSide[edgeIndex, 1];
        List<int> side1 = sideExitMap[srcSide];
        List<int> side2 = sideExitMap[tarSide];
        for (int index1 = 0; index1 < side1.Count; index1++)
        {
            for (int index2 = 0; index2 < side2.Count; index2++)
            {
                //TODO hard code of gridCount-1
                /**这段太蠢了- -*/
                if (side1[index1] == ((gridCount - 1) - side2[index2]))
                {
                    Debug.Log("match in " + srcSide + " " + side1[index1] + " " + tarSide + " " + side2[index2]);
                    foreach (KeyValuePair<string, KeyPoint> pair in srcPointList)
                        //WARNING hard code of C3<---the index of number in the string
                        if (pair.Value.gridIndex == side1[index1] && (pair.Value.belongToSide() == srcSide))
                            list.Add(pair.Value);

                    foreach (KeyValuePair<string, KeyPoint> pair in tarPointList)
                    {
                        Debug.Log("consistent is " + pair.Value.belongToSide() + " " + pair.Value.gridIndex + " " + edgeSide[edgeIndex, 0] + " " + edgeSide[edgeIndex, 1]);
                        if (pair.Value.gridIndex == side2[index2] && (pair.Value.belongToSide() == tarSide))
                            list.Add(pair.Value);
                    }
                }
                else
                {
                    Debug.Log("not match in " + side1[index1] + " " + side2[index2]);
                }
            }
        }
        return list;
    }
    public List<string> getRoute(string srcRouteName, string tarRouteName, int[,] srcMap)
    {
        Debug.Log("In getRoute " + srcRouteName + " " + tarRouteName);
        int src = srcRouteName.Contains("Point") ? srcRouteName[6] - '0' : srcRouteName[2] - '0';
        int tar = tarRouteName.Contains("Point") ? tarRouteName[6] - '0' : tarRouteName[2] - '0';
        //TODO magic number of size of map
        Debug.Log(srcRouteName + " " + src + " " + tarRouteName + " " + tar);
        int[] visited = new int[100];
        bool found = false;
        List<int> route = DFS(ref visited, srcMap, src, tar, new List<int>(), ref found);
        List<string> strRoute = new List<string>();
        for (int i = 0; i < route.Count; i++)
        {
            strRoute.Add("Point" + getSurfaceName(srcRouteName) + route[i]);
        }

        //Debug.Log("DFS end " + route.Count);
        //for (int i = 0; i < route.Count; i++)  Debug.Log("final route "+route[i]);
        if (route.Count == 0) return new List<string>();
        if (route[route.Count - 1] == tar) return strRoute;
        else return new List<string>();
    }
    private List<int> DFS(ref int[] visited, int[,] srcMap, int src, int tar, List<int> route, ref bool found)
    {
        visited[src] = 1;

        string dfsstr = "";
        for (int i = 0; i < route.Count; i++)
        {
            dfsstr += route[i];
        }
        //Debug.Log("DFS " + dfsstr+" "+src);
        route.Add(src);
        if (tar == src)
        {
            found = true;
            return route;
        }
        for (int i = 0; i < mapSize; i++)
        {
            if (srcMap[src, i] != 0 && visited[i] != 1)
            {
                //Debug.Log(src + " to " + i);
                DFS(ref visited, srcMap, i, tar, route, ref found);
                if (found) return route;
            }
        }
        //Debug.Log("off " + src);
        route.RemoveAt(route.Count - 1);
        visited[src] = 0;
        return route;
    }
    /**there are two name of route -- Point[X][n] and X-[n]-[n]*/
    //TODO change all the hard code because of the route name inconsistent
    public static string getSurfaceName(string routeName)
    {
        Debug.Log("here get surface name " + routeName);
        if (routeName.Contains("-"))
            return routeName[0] + "";
        else if (routeName.Contains("Plane"))
            return routeName[5] + "";
        else
            return routeName[5] + "";
    }

    public List<string> findRoute(Transform srcPos, string srcRouteName, Transform tarPos, string tarRouteName)
    {
        List<string> route = new List<string>();
        string srcSurfaceName = getSurfaceName(srcRouteName);
        string tarSurfaceName = getSurfaceName(tarRouteName);
        int[,] srcSurfaceMap = surfaceMap[srcSurfaceName];
        int edgeIndex = -1;
        if (isSameRoute(srcRouteName, tarRouteName))
        {
            //TODO same route, just go ahead
        }
        else if (isSameSurface(srcSurfaceName, tarSurfaceName))
        {
            route = getRoute(srcRouteName, tarRouteName, srcSurfaceMap);
        }
        else if ((edgeIndex = isAdjacent(srcSurfaceName, tarSurfaceName)) != -1)
        {
            /**stage 1
             *    judge whether the two sides are reachable
             */
            Debug.Log("sides are " + edgeSide[edgeIndex, 0] + " " + edgeSide[edgeIndex, 1]);
            /**WARNING the list is consist of pair points(ex. A0,C3,A1,C2)*/
            List<KeyPoint> pointList =
                getSideMatchExitList(edgeIndex, srcSurfaceName, tarSurfaceName);
            string test = "";
            for (int i = 0; i < pointList.Count; i++) test += pointList[i].toString();
            Debug.Log("pointList is " + test);
            for (int index = 0; index < pointList.Count; index += 2)
            {
                /**stage 2
                 *    find route to exit points(perhaps not only)
                 */
                List<string> routeListInSrc = getRoute(srcRouteName, pointList[index].pointName, srcSurfaceMap);
                if (routeListInSrc.Count == 0)
                    continue;
                else
                {
                    /**stage 3
                     *     find whether the exit points which reachable to source point can reach target point
                     */
                    Debug.Log("in stage 3");
                    List<string> routeListInTar = getRoute(pointList[index + 1].pointName, tarRouteName, surfaceMap[tarSurfaceName]);
                    if (routeListInTar.Count != 0)
                    {
                        globalRoute.setSurfaceCount(2);
                        route = routeListInSrc;
                        for (int i = 0; i < routeListInTar.Count; i++) route.Add(routeListInTar[i]);
                        break;
                    }
                }
            }

        }

        if (route.Count != 0)
        {
            showRoute(route);
            globalRoute.reset(route, srcRouteName, tarPos, tarRouteName);
            globalRoute.setOnGoing(true);
            //TODO set surface name
            globalRoute.cntKeyPointMap = keyPointMap[srcSurfaceName];
        }
        return route;
    }
    public void rotateSurface(string surfaceName)
    {
        Debug.Log(edgeSide.Length / 2);
        for (int edgeIndex = 0; edgeIndex < edgeSide.Length / 2; edgeIndex++)
        {
            for (int sideIndex = 0; sideIndex < 2; sideIndex++)
            {
                if (edgeSide[edgeIndex, sideIndex] == null)
                    continue;
                if (edgeSide[edgeIndex, sideIndex].Contains(surfaceName))
                {
                    //WARNING hard code
                    string sideName = edgeSide[edgeIndex, sideIndex];
                    Debug.Log("ori rotate name " + sideName);
                    sideName = sideName.Substring(0, sideName.Length - 1) + (char)(sideName[sideName.Length - 1] == '0' ? '2' : sideName[sideName.Length - 1] - 1);
                    Debug.Log("rotate of " + sideName);
                    edgeSide[edgeIndex, sideIndex] = sideName;
                }
            }
        }
    }
    void Start()
    {
        StreamReader sr = new StreamReader("Assets/Level1/map.txt", Encoding.Default);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            //Debug.Log(line.ToString());
        }
        int[,] Amap = {
                          {1,0,0,1,0,0,0,0},//0
                          {0,1,1,0,0,0,0,0},//1
                          {0,1,1,0,0,0,0,0},//2
                          {1,0,0,1,0,0,0,0},//3
                          {0,0,0,0,0,0,0,0},//4
                          {0,0,0,0,0,0,0,0},//5
                          {0,0,0,0,0,0,0,0},//6
                          {0,0,0,0,0,0,0,0},//7
                     };

        int[,] Bmap = {
                          {1,0,0,0,0,0,1,0},//0
                          {0,1,0,0,0,0,1,0},//1
                          {0,0,1,0,0,0,1,0},//2
                          {1,0,0,1,0,0,1,0},//3
                          {0,0,0,0,1,0,1,0},//4
                          {0,0,0,0,0,1,1,0},//5
                          {1,1,1,1,1,1,1,0},//6
                          {0,0,0,0,0,0,0,0},//7
                     };
        surfaceMap.Add("A", Amap);
        surfaceMap.Add("B", Bmap);
        //TODO init position of objects stand for points
        Dictionary<string, KeyPoint> Apoints = new Dictionary<string, KeyPoint>();
        int[] AgridIndex = { 2, 4, 2 , 2 };
        int i = 0;
        for (; i < 4; i++)
        {
            string pointName = "PointA" + i;
            List<double> pos = new List<double>();
            pos.Add(GameObject.Find(pointName).transform.position.x);
            pos.Add(GameObject.Find(pointName).transform.position.y);
            pos.Add(GameObject.Find(pointName).transform.position.z);
            Apoints.Add(pointName, new KeyPoint(pos, i, AgridIndex[i], false, "A", pointName));
        }
        keyPointMap.Add("A", Apoints);

        Dictionary<string, KeyPoint> Bpoints = new Dictionary<string, KeyPoint>();
        int[] BgridIndex = { 1, 3, 2,4, 2,4 };
        i = 0;
        for (; i < 6; i++)
        {
            string pointName = "PointB" + i;
            List<double> pos = new List<double>();
            pos.Add(GameObject.Find(pointName).transform.position.x);
            pos.Add(GameObject.Find(pointName).transform.position.y);
            pos.Add(GameObject.Find(pointName).transform.position.z);
            Bpoints.Add(pointName, new KeyPoint(pos, i, BgridIndex[i], false, "B", pointName));
        }
        for (; i < 7; i++)
        {
            string pointName = "PointB" + i;
            List<double> pos = new List<double>();
            pos.Add(GameObject.Find(pointName).transform.position.x);
            pos.Add(GameObject.Find(pointName).transform.position.y);
            pos.Add(GameObject.Find(pointName).transform.position.z);
            Bpoints.Add(pointName, new KeyPoint(pos, -1, -1, true, "B", pointName));
        }
        keyPointMap.Add("B", Bpoints);

        /*
        int[] CgridIndex = { 4, 4, 5, 2 };
        i = 0;
        Dictionary<string, KeyPoint> Cpoints = new Dictionary<string, KeyPoint>();
        for (; i < 4; i++)
        {
            string pointName = "PointC" + i;
            List<double> pos = new List<double>();
            pos.Add(GameObject.Find(pointName).transform.position.x);
            pos.Add(GameObject.Find(pointName).transform.position.y);
            pos.Add(GameObject.Find(pointName).transform.position.z);
            Cpoints.Add(pointName, new KeyPoint(pos, i, CgridIndex[i], false, "C", pointName));
        }
        for (; i < 6; i++)
        {
            string pointName = "PointC" + i;
            List<double> pos = new List<double>();
            pos.Add(GameObject.Find(pointName).transform.position.x);
            pos.Add(GameObject.Find(pointName).transform.position.y);
            pos.Add(GameObject.Find(pointName).transform.position.z);
            Cpoints.Add(pointName, new KeyPoint(pos, -1, -1, true, "C", pointName));
        }
        keyPointMap.Add("C", Cpoints);
        */
        List<int> a0 = new List<int>();
        a0.Add(AgridIndex[0]);
        a0.Add(AgridIndex[1]);
        List<int> a1 = new List<int>();
        a1.Add(AgridIndex[2]);
        List<int> a2 = new List<int>();
        a2.Add(AgridIndex[3]);
        sideExitMap.Add("A0", a0);
        sideExitMap.Add("A1", a1);
        sideExitMap.Add("A2", a2);

        List<int> b0 = new List<int>();
        b0.Add(BgridIndex[0]);
        b0.Add(BgridIndex[1]);
        List<int> b1 = new List<int>();
        b1.Add(BgridIndex[2]);
        b1.Add(BgridIndex[3]);
        List<int> b2 = new List<int>();
        b2.Add(BgridIndex[4]);
        b2.Add(BgridIndex[5]);
        sideExitMap.Add("B0", b0);
        sideExitMap.Add("B1", b1);
        sideExitMap.Add("B2", b2);
        /**edge index map between surfaces*/
        surfaceEdgeMap.Add("AB", 0);
        /**edge-side array */
        edgeSide = new string[6, 2];
        edgeSide[0, 0] = "A2";
        edgeSide[0, 1] = "B0";

        globalRoute = new Route(keyPointMap, character);
    }

    public void setPointsPosition()
    {
        int i = 0;
        string surface = "A";
        for (; i < 4; i++)
        {
            string pointName = "PointA" + i;
            List<double> pos = new List<double>();
            pos.Add(GameObject.Find(pointName).transform.position.x);
            pos.Add(GameObject.Find(pointName).transform.position.y);
            pos.Add(GameObject.Find(pointName).transform.position.z);
            keyPointMap[surface][pointName].setPosition(pos);
        }
        i = 0;
        surface = "B";
        for (; i < 6; i++)
        {
            string pointName = "PointB" + i;
            List<double> pos = new List<double>();
            pos.Add(GameObject.Find(pointName).transform.position.x);
            pos.Add(GameObject.Find(pointName).transform.position.y);
            pos.Add(GameObject.Find(pointName).transform.position.z);
            keyPointMap[surface][pointName].setPosition(pos);
        }
        for (; i < 7; i++)
        {
            string pointName = "PointB" + i;
            List<double> pos = new List<double>();
            pos.Add(GameObject.Find(pointName).transform.position.x);
            pos.Add(GameObject.Find(pointName).transform.position.y);
            pos.Add(GameObject.Find(pointName).transform.position.z);
            keyPointMap[surface][pointName].setPosition(pos);
        }
        globalRoute.setkeyPointMap(keyPointMap);
    }
    // Update is called once per frame
    void Update()
    {
        //character.test();
        if (globalRoute.isGoing())
        {
            if (!globalRoute.hasRoute())
            {
                character.setFinalPosition(globalRoute.getFinalPosition());
                character.setFinalRoute(globalRoute.getFinalRoute());
                globalRoute.setOnGoing(false);
            }
            else if (character.isReach(globalRoute.nextPoint()))
            {
                Debug.Log("Here Is Reach");
                globalRoute.goNextPoint();
            }
            else character.forward(globalRoute.nextPoint());
            /**瞬移到另一个面的可达点，这个放在最后的插入方式比较丑，等待重构*/
            /*
            if(globalRoute.isForwardingNextSurface())
                character.
             */
        }
    }
}
