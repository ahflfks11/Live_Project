// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("P6wwVpnpLf+wEPqur3ZqQi9++Qnb/QYnqb6sgnbF3XQLd9XLNUEfQRxqyPS8o+kJrxBpAJ87a4l5WDzb8BZmMYj4CLjXkBRUHrrBYs2+RhqVQe2GYuFMfE2apXIRpBsZESEgG3Dz/fLCcPP48HDz8/Jr4zMEwmBIdpBssRexyHwuB4VR+nr5NmEFL1INrNQWfIy9+ore8Uy4qB1ah7T5yv+rekgGCDGPbOb1K9yvegvFbBGNc8FsC2v8LDxv/FXgB2OrIv+VZ/jCcPPQwv/0+9h0unQF//Pz8/fy8SgrwtcZl/ErpTSl2SNFaf3/t+TKkJxMApFMpzWE8bomVjvwky0q76GAlIWbmI0QXtbjiEXGIETk+LSHXZG86YHwVEmca/Dx8/Lz");
        private static int[] order = new int[] { 3,2,7,8,13,7,10,7,12,10,12,11,12,13,14 };
        private static int key = 242;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
