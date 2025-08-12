import { gql } from "@apollo/client";

export const GET_ORDERS = gql`
  query ($input: OrderPagedInput!) {
  orders(input: $input) {
    items {
      id
      customerName
      totalAmount
      createdAt
    }
    totalCount
    pageNumber
    pageSize
  }
}
`;