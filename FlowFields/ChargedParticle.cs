using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class ChargedParticle
    {
        public Vector Location = Vector.Zero;
        public bool Negative = false;
        public double Charge = 1;

        public ChargedParticle(Vector location)
        {
            this.Location = location;
        }

        public ChargedParticle(Vector location, bool negative, double charge = 1)
        {
            this.Location = location;
            this.Negative = negative;
            this.Charge = charge;
        }
    }
}
