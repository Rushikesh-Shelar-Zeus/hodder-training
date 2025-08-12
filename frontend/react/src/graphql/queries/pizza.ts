import { gql } from "@apollo/client";

export const GET_PIZZAS = gql`
  query($input: PizzaPagedInput!){
  pizzas(input: $input){
    items{
      id
      name
      price
      isGlutenFree
    }
    totalCount
    pageNumber
    pageSize
  }
}
`;