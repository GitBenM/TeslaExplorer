module.exports = {
    mode: "production", // "production" | "development" | "none"
    // Chosen mode tells webpack to use its built-in optimizations accordingly.
    entry: { game: "./app.js", vehicle: "./vehicle.js" }, // string | object | array
    // defaults to ./src
    // Here the application starts executing
    // and webpack starts bundling
    context: __dirname,
    output: {
        // options related to how webpack emits results
        path: __dirname + "/dist", // path.resolve(__dirname, "dist"), // string
        // the target directory for all output files
        // must be an absolute path (use the Node.js path module)
        filename: "[name].js", // string
        // the filename template for entry chunks
        //publicPath: "/assets/", // string
        // the url to the output directory resolved relative to the HTML page
        //library: "tesla-explorer", // string,
        // the name of the exported library
        //libraryTarget: "umd", // universal module definition
        // the type of the exported library
        /* Advanced output configuration (click to show) */
        /* Expert output configuration (on own risk) */
    },
    watch: true,
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /(node_modules)/,
                use: {
                    loader: "babel-loader",
                    options: {
                        presets: ['@babel/preset-env', '@babel/preset-react']
                    }
                }
            }
        ]
    }
}