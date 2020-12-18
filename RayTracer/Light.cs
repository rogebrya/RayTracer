using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Light {
        private Tuple position;
        private Color intensity;
        public Light(Tuple position, Color intensity) {
            this.position = position;
            this.intensity = intensity;
        }

        public Tuple Position {
            get { return position; }
            set { position = value; }
        }

        public Color Intensity {
            get { return intensity; }
            set { intensity = value; }
        }

        public static Light PointLight(Tuple position, Color intensity) {
            return new Light(position, intensity);
        }

        /// <summary>
        /// ///////////////////////////////////////
        /// </summary>
        /// <param name="position"></param>
        /// <param name="intensity"></param>
        /// <returns></returns>
        public static Light SpotLight(Tuple position, Color intensity) {
            return new Light(position, intensity);
        }

        // Equality Stuff
        public static bool operator ==(Light t1, Light t2) {
            return t1.Position == t2.Position && t1.Intensity == t2.Intensity;
        }

        public static bool operator !=(Light t1, Light t2) {
            return !(t1.Position == t2.Position && t1.Intensity == t2.Intensity);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Light)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (int)Math.Pow(((position.X * position.Z) * 397), position.Y) + (int)intensity.Red;
            }
        }

        public bool Equals(Light other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Position == other.Position && Intensity == other.Intensity;
        }
    }
}
