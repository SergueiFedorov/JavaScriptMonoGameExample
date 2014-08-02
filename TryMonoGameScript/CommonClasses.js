sprite = function (scene, vector, rotation) {

    this.position = vector;
    this.rotation = rotation;
    this.scene = scene;
    this.backendID = 0;

    this.setTexture = function(texture)
    {
        setTextureFor(this.backendID, texture);
    }

    this.updateObject = function () {


       // updateSprite( this.backendID, this.position.x, this.position.y, this.rotation );
    }

    this.Initialize = function () { }
    this.update = function (gameTime)
    {
        updateSprite(this.backendID, this.position.x, this.position.y, this.rotation);
    }

}

scene = function () {

    this.objectCount = 0;
    this.objectIndex = 0;

    this.sprites = [];
    this.addSprite = function (texture, vector, rotation) {

        var newSprite = new sprite(this, vector, rotation);

        this.sprites[this.objectCount] = newSprite;
        this.objectCount++;

        newSprite.backendID = MonoGame_AddSprite(this, vector.x, vector.y, rotation);

        return newSprite;
    }

    this.removeSprite = function (sprite) {
        
        for (var x = 0; x < this.sprites.length; x++) {
            if (this.sprites[x] === sprite) {
                delete sprites[x];
            }
        }

    }

    this.updateInternal = function (gameTime) {
        this.delegate.update(gameTime);

        for (var x = 0; x < this.sprites.length; x++) {
            this.sprites[x].update(gameTime);
        }
    }
}