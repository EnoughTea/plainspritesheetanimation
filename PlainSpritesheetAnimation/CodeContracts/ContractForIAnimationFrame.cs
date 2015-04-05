using System.Diagnostics.Contracts;

namespace PlainSpritesheetAnimation {
    [ContractClassFor(typeof(IAnimationFrame))]
    internal abstract class ContractForIAnimationFrame : IAnimationFrame {
        public TextureRegion Source { get; set; }

        public float Duration {
            get {
                Contract.Ensures(Contract.Result<float>() >= 0);
                return 0;
            }
            set {
                Contract.Requires(value >= 0, "Frame duration must be >= 0.");
            }
        }

        public TexturePoint Origin { get; set; }

        public IAnimationFrame Clone() {
            Contract.Ensures(Contract.Result<IAnimationFrame>() != null);
            return null;
        }
    }
}