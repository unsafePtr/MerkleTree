
Simple binary Merkle tree to calculate the root.

When I made this implementation my main goal was to understand why the Merkle tree is used in Bitcoin and how it works. The implementation is simple and is far away from production ready code.

One of the use cases of the Merkle tree is signature verification. In distributed systems when you receive data from different places it's one of the ways to verify that data is valid and not corrupted. 

Originally Merkle tree [is not required to be binary](https://bitcoin.stackexchange.com/questions/42624/are-bitcoin-merkle-trees-always-binary). But all implementations usually implement it as binary. Thus we can construct a shorter path to the root. If data is transferred over the network this will mean that fewer amount of data will be sent.

The implementation duplicates hash if the number of leaves is odd. In [Bitcoin this case is foreseeable](https://github.com/bitcoin/bitcoin/blob/c94852e7912eb0881eb38af28cd2e3076e53e393/src/consensus/merkle.cpp). Each leaf in bitcoin's Merkle tree is a hash of transaction. Repeating it will result in double-spent of the transaction.
