(function () {

    for (var counter = 0; counter < 10; counter++) {

        var sprite = new Sprite();
        sprite.SetTexture("sprite_logo");
        currentScene.addSprite(sprite);

        sprite.x = counter * 100;
        sprite.scale = 0.2;

        sprite.someFloat = 5.0;

        sprite.update = function (gametime) {

            this.rotation += 0.001;
            this.y += 0.01 * gametime;

        }

        registerUpdateFunction(sprite);

    }

}());
