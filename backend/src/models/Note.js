import mongoose from "mongoose";

// 1st step- you need to create a schema
// 2nd step- you would create a model based off of that schema

const noteSchema = new mongoose.Schema(
    {
    title: {
        type: String,
        required: true,
    },
    content: {
        type: String,
        required: true,
    },
    },
    {timestamps:true} //createdAt, updatedAt
);

const Note = mongoose.model("Note", noteSchema);

export default Note;