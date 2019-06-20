namespace UnityEngine.Global
{
    public static class GlobalInput
    {
        public static Vector2 MousePosition =>
            new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
    }
}