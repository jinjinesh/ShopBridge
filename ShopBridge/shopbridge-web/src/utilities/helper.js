export default {
    guidIsNullOrEmpty (guid) {
        return (guid === '00000000-0000-0000-0000-000000000000' || guid === null || guid === undefined);
    }
};
