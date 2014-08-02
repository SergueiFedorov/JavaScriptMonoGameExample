/*
scene = function () {

    this.objectCount = 0;
    this.objectIndex = 0;

    this.sprites = [];
    this.addSprite = function (texture, vector, rotation) {
        var newSprite = new sprite(vector.x, vector.y, rotation);
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
           // this.sprites[x].update(gameTime);
        }
    }
}*/