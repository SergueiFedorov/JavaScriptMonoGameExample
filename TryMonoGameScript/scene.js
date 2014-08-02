scene = function () {

    var currentSprite = null;

    this.initialize = function (gameTime) {

        currentSprite = currentScene.addSprite('', new Vector2(0, 0), 0);
        currentSprite.setTexture("Sprite_logo.jpg");

    }

    this.update = function (gameTime) {
        currentSprite.position.x += 0.5;
        currentSprite.rotation += 0.001;
    }

}

currentScene.delegate = new scene();