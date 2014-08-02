
function Vector2(x, y) {

    this.x = x;
    this.y = y;

    this.add = function (vector) {
        var returnVector = new Vector2(this.x + vector.x, this.y + vector.y);
        return returnVector;
    }

    this.sub = function (vector) {
        var returnVector = new Vector2(this.x - vector.x, this.y - vector.y);
        return returnVector;
    }

    this.mult = function (scalar) {
        var returnVector = new Vector2(this.x * scalar, this.y * scalar);
        return returnVector;
    }

    this.length = function () {
        return Math.sqrt(x * x + y * y);
    }

    this.norm = function () {
        var length = this.length();
        return new Vector2(this.x / length, this.y / length);
    }
}