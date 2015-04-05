# Plain sprite-sheet animation library

Plain sprite-sheet animation library is low-level library for a sprite-sheet 2D animation support (who would have thought?). 
Manages all information needed to draw a sprite to screen; how exactly it is drawn is up to user.

I made this thing for myself, so it is not supposed to be a complex framework, but perhaps someone would find it useful.


## Well, what is it?

Animation is represented as a sequence of frames. These could be created manually or loaded from [TexturePacker](https://www.codeandweb.com/texturepacker).
Then you can set some properties defining how resulting sequence should be played. Call sequence update method in your update logic, draw its current frame if sequence is visible, and that's it.
  * Plain and simple to use and extend.
  * Supports [TexturePacker](https://www.codeandweb.com/texturepacker)'s generic XML format.
  * Animation sequences are serializable through DataContractSerializer.


###Creating animation sequences with TexturePacker's generic XML texture atlas description

I really like TexturePacker, so I will show it first. Just for the reference, TexturePacker's generic XML file looks like this:
    <?xml version="1.0" encoding="UTF-8"?>
	<!-- Created with TexturePacker http://texturepacker.com-->
	<!-- $TexturePacker:SmartUpdate:78d3aff951abb5c82993a205c536379f$ -->
	<!--Format:
	n  => name of the sprite
	x  => sprite x pos in texture
	y  => sprite y pos in texture
	w  => sprite width (may be trimmed)
	h  => sprite height (may be trimmed)
	oX => sprite's x-corner offset (only available if trimmed)
	oY => sprite's y-corner offset (only available if trimmed)
	oW => sprite's original width (only available if trimmed)
	oH => sprite's original height (only available if trimmed)
	r => 'y' only set if sprite is rotated
	-->
    <TextureAtlas imagePath="whole texture atlas.png" width="1024" height="2048">
        <sprite n="walking 1.png" x="0" y="0" w="32" h="48"/>
		<sprite n="walking 2.png" x="32" y="0" w="32" h="48"/>
		<sprite n="walking 3.png" x="64" y="0" w="32" h="48"/>
		<sprite n="standing.png" x="96" y="0" w="59" h="83"/>
	    ...
    </TextureAtlas>

Given this file, animation sequence could be created like this:

	using (var atlasFileStream = File.OpenRead(atlasXmlFile)) {
        var atlasData = TexturePackerAtlas.Load(atlasFileStream);
		// Now that TP's data is loaded, you can do whatever with it. Lets create sequences from it:
		var animationSequences = atlasData.CreateAnimationSequences().ToList();

		// 'animationSequences' will contain two sequences: 1) "walking" with 3 frames; 2) "standing" with 1 frame.
		var walking = animationSequences.FindSequenceByName("walking");
		var standing = animationSequences.FindSequenceByName("standing");
			
		// Amount of time each frame in sequence is shown is 0 right now, since these are out of TexturePacker's scope.
		// It possible to set duration for entire sequence:
		walking.SetDuration(1f);
		// 'walking' consists of 3 frames with 0 second durations, this new duration will be distributed evenly.
		// Each frame's new duration would be 1/3.
        // By the way, if frame durations would have been set to something other than 0,
		// they would be scaled to new duration, keeping timing ratio between old and new durations.

		// Lets run 'walking' sequence in a loop:
		walking.AnimationType = AnimationType.Looping;

		// And 'standing' needs its only frame to be shown continously:
		standing.AnimationType = OnceHoldLast;

		// I want 'standing' sequence to start when 'walking' one stops:
		walking.Stopped += (sequence, _) => standing.Start();
		// 'Stopped' event will occur when sequence is stopped. It can happen either when its time is up, 
		// or when its 'Animating' property is changed by Stop() call or direct change by user.
		// 'Started' event works in similar way.
    }


###Creating animation sequences through code:

Same sequences as in example above can be created through code.

	string walkingName = "walking";
	var walkingFrames = new List<AnimationFrame> {
		new AnimationFrame(new TextureRegion(0, 0, 32, 48), 0.333f),
		new AnimationFrame(new TextureRegion(32, 0, 32, 48), 0.333f),
		new AnimationFrame(new TextureRegion(64, 0, 32, 48), 0.333f),
	};

	var walking = new AnimationSequence(walkingName, walkingFrames);
	var standing = new AnimationSequence("standing", 
										 new[] { new AnimationFrame(new TextureRegion(96, 0, 59, 83), 0) });


###One last thing

Animations need to know how much time has passed, so they need to be updated somewhere:

	private void YourLogicUpdate(float deltaTimeBetweenUpdatesInSeconds) {
		...
		animationSequences.Update(deltaTimeBetweenUpdatesInSeconds);
	}

There is no built-in renderer, so you will need to have your own. Then rendering logic could look like this:

	private void YourDraw() {
		var sequencesWhichNeedsDrawing = yourAnimationSequences.GetVisibleSequences();
		foreach (var sequence in sequencesWhichNeedsDrawing) {
			var texture = YourTextureManager.GetTexture(sequence.TextureId);
			var frameToDraw = sequence.CurrentFrame;
			var textureSourceTexels = frameToDraw.Source;
			var frameDrawOffset = frameToDraw.Origin;
			var screenPosition = yourObjectPosition + frameDrawOffset;
			yourSpriteBatch.Draw(texture, screenPosition, textureSourceTexels, Color.White);
        }
	}


### Unit tests NuGet references

You may notice that NuGet packages are not in the repository, so do not forget to set up package restoration in Visual Studio:

Tools menu → Options → Package Manager → General → "Allow NuGet to download missing packages during build" should be selected. 

If you have a build server then it needs to be setup with an environment variable 'EnableNuGetPackageRestore' set to true.

If you do not use Visual Studio, then I guess that you already know how to restore packages from console.