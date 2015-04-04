using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Unisa {
    [ContractClassFor(typeof(IAnimation))]
    internal abstract class ContractForIAnimation : IAnimation {
        public string TextureId { get; set; }

        public HashSet<IAnimationSequence> Sequences {
            get {
                Contract.Ensures(Contract.Result<HashSet<IAnimationSequence>>() != null);
                return null;
            }
        }

        public IAnimationSequence FindSequence(string sequenceName) {
            return null;
        }

        public IEnumerable<IAnimationSequence> GetAnimatingSequences() {
            Contract.Ensures(Contract.Result<IEnumerable<IAnimationSequence>>() != null);
            return null;
        }

        public IEnumerable<IAnimationSequence> GetVisibleSequences() {
            Contract.Ensures(Contract.Result<IEnumerable<IAnimationSequence>>() != null);
            return null;
        }

        public TextureSize GetMaxVisibleFrameSize() {
            return TextureSize.Zero;
        }

        public void Update(float delta) {
            
        }

        public IAnimation Clone() {
            Contract.Ensures(Contract.Result<IAnimation>() != null);
            return null;
        }
    }
}