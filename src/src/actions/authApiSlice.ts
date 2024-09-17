import { apiSlice } from "./apiSlice";
export const authApiSlice = apiSlice.injectEndpoints({
    endpoints:builder=>({
        login:builder.mutation({
            query:credentials=>({
                url:'api/auth',
                headers:{
                    'Accept': 'application/json',
                    'Content-Type': 'application/json' 
                },
                method:'POST',
                body:{...credentials}
            })
        })
    })
})
export const {
    useLoginMutation 
}= authApiSlice

//Mutation is for POST,PUT,DELETE
